using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StreamingMovie.Application.Interfaces.ExternalServices.Storage
{
    public interface IStorageHandler
    {
        Task<(string UploadId, List<(int PartNumber, string Url)>)> StartMultipartUploadAsync(
            string fileName,
            long fileSize
        );

        Task CompleteMultipartUploadAsync(
            string fileName,
            string uploadId,
            List<(int PartNumber, string ETag)> parts
        );
    }
}
