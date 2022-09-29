using ServiceStack.DataAnnotations;

namespace ClipHost.ServiceModel.Types
{
    [TableUp(2)]
    public class TwitchClip : TablesUp
    {
        [Required]
        [References(typeof(Streamer))]
        public int StreamerId { get; set; } = 0;
        public string TwitchClipId { get; set; }

    }
}
