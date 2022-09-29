using ServiceStack.DataAnnotations;

namespace ClipHost.ServiceModel.Types
{
    [TableUp(2)]
    public class ClipFrameEvent : TablesUp
    {

        [References(typeof(TwitchClip))]
        public int TwitchClipId { get; set; } = 0;
        public int FrameNumber { get; }
        public int FPS { get; }
        public int Second { get; }
        public string EventName { get; }


    }
}
