using ServiceStack.DataAnnotations;
using System.Collections.Generic;

namespace ClipHost.ServiceModel.Types
{
    [TableUp(3)]

    [CompositeIndex("StreamerId", "MonitoredVideoStreamId")]
    public class StreamStatus : TablesUp
    {

        [Required]
        [References(typeof(Streamer))]
        public int StreamerId { get; set; } = 0;
        [Required]
        [References(typeof(MonitoredVideoStream))]
        public int MonitoredVideoStreamId { get; set; } = 0;

        [Required]
        public Dictionary<string, string> StatusValues { get; set; } = new Dictionary<string, string>();

    }
}
