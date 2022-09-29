using ServiceStack.DataAnnotations;

namespace ClipHost.ServiceModel.Types
{
    [TableUp(1)]
    public class MonitoredStreamType : TablesUp
    {

        [Required]
        public string StreamType { get; set; } = ""; //twitch, yt etc
    }
}
