using ServiceStack.DataAnnotations;

namespace ClipHost.ServiceModel.Types
{
    [TableUp(3)]
    public class TwitchClipError : TablesUp, IErrors
    {
        [References(typeof(TwitchClip))]
        public int TwitchClipId { get; set; }
        public string Message { get; set; }
        public string StackTrace { get; set; }
        public string Context { get; set; }
        public string Location { get; set; }
    }
}
