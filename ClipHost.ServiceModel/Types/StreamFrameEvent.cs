using ServiceStack.DataAnnotations;

namespace ClipHost.ServiceModel.Types
{
    [TableUp(2)]
    public class StreamFrameEvent : TablesUp
    {

        [References(typeof(TwitchStream))]
        public int TwitchStreamId { get; set; } = 0;
        public int FrameNumber { get; }
        public int FPS { get; }
        public int Second { get; }
        public string EventName { get; }


    }
}
