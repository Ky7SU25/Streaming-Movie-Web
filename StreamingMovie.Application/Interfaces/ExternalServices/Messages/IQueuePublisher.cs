using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StreamingMovie.Application.Interfaces.ExternalServices.Messages
{
    public interface IQueuePublisher
    {
        Task Publish(VideoProcessingMessage message);
    }

    public class VideoProcessingMessage
    {
        public string MovieId { get; set; }
        public string MovieVideoId { get; set; }
        public string PathStorage { get; set; }
    }
}
