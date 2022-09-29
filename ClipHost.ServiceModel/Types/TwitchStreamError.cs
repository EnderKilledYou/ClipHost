using ServiceStack.DataAnnotations;

namespace ClipHost.ServiceModel.Types
{
    [TableUp(3)]
    public class TwitchStreamError : TablesUp, IErrors
    {
        [References(typeof(TwitchStream))]
        public int TwitchStreamId { get; set; }
        public string Message { get; set; }
        public string StackTrace { get; set; }
        public string Context { get; set; }
        public string Location { get; set; }
    }
}
