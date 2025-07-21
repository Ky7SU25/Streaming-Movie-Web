using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.Extensions.Options;
using StreamingMovie.Application.Interfaces.ExternalServices.Storage;

namespace StreamingMovie.Infrastructure.ExternalServices.Storage
{
    public class MinioStorageHandler : IStorageHandler
    {
        private readonly IAmazonS3 _s3;
        private readonly MinioOptions _options;

        public MinioStorageHandler(IOptions<MinioOptions> options)
        {
            _options = options.Value;

            _s3 = new AmazonS3Client(
                _options.AccessKey,
                _options.SecretKey,
                new AmazonS3Config { ServiceURL = _options.Endpoint, ForcePathStyle = true }
            );
        }

        public async Task<(
            string UploadId,
            List<(int PartNumber, string Url)>
        )> StartMultipartUploadAsync(string fileName, long fileSize)
        {
            var init = await _s3.InitiateMultipartUploadAsync(
                new InitiateMultipartUploadRequest { BucketName = _options.Bucket, Key = fileName }
            );

            var uploadId = init.UploadId;
            var partSize = 5 * 1024 * 1024;
            var partCount = (int)Math.Ceiling((double)fileSize / partSize);

            var urls = new List<(int, string)>();
            for (int partNumber = 1; partNumber <= partCount; partNumber++)
            {
                var req = new GetPreSignedUrlRequest
                {
                    BucketName = _options.Bucket,
                    Key = fileName,
                    UploadId = uploadId,
                    PartNumber = partNumber,
                    Verb = HttpVerb.PUT,
                    Expires = DateTime.UtcNow.AddMinutes(30),
                    Protocol = Protocol.HTTP
                };
                urls.Add((partNumber, _s3.GetPreSignedURL(req)));
            }

            return (uploadId, urls);
        }

        public async Task CompleteMultipartUploadAsync(
            string fileName,
            string uploadId,
            List<(int PartNumber, string ETag)> parts
        )
        {
            var completeReq = new CompleteMultipartUploadRequest
            {
                BucketName = _options.Bucket,
                Key = fileName,
                UploadId = uploadId,
                PartETags = parts.Select(p => new PartETag(p.PartNumber, p.ETag)).ToList()
            };

            await _s3.CompleteMultipartUploadAsync(completeReq);
        }
    }
}
