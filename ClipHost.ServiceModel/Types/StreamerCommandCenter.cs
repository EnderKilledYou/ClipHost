using ServiceStack.DataAnnotations;

namespace ClipHost.ServiceModel.Types
{
    [TableUp(2)]
    [CompositeKey("CommandCenterId", "StreamerId")]
    public class StreamerCommandCenter : TablesUp
    {


        [Required]
        [References(typeof(Streamer))]
        public int StreamerId { get; set; } = 0;

        [Required]
        [References(typeof(CommandCenter))]

        public int CommandCenterId { get; set; } = 0;
    }
}
