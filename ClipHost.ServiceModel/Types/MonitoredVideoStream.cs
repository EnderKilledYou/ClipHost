using ServiceStack.DataAnnotations;

namespace ClipHost.ServiceModel.Types
{
    [TableUp(2)]
    public class MonitoredVideoStream : TablesUp
    {

        public string ThumbNail { get; set; }
        [Required]
        public string StreamId { get; set; } //twitch stream id, yt etc
        [Required]
        [References(typeof(MonitoredStreamType))]
        public int MonitoredStreamTypeId { get; set; } = 0;
    }
}
