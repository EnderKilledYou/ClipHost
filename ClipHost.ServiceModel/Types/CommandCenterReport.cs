using BlazorQueue;
using ServiceStack.DataAnnotations;

namespace ClipHost.ServiceModel.Types
{
    [TableUp(4)]
    [CompositeKey("StreamerCommandCenterId", "Name")]
    public class CommandCenterReport : TablesUp, IQueueReport
    {
        [SearchField]
        public string Name { get; set; } = "";


        public int TotalProcessed { get; set; } = 0;
        public int AverageMilliSeconds { get; set; } = 0;
        public int HighMilliSeconds { get; set; } = 0;
        public int LowMilliSeconds { get; set; } = 0;
        public int MaxSize { get; set; } = 0;

        

        public int _processId { get; set; } = 0;
        public int ProcessId => _processId;

        public int Size { get; set; } = 0;
        [Required]
        [References(typeof(StreamerCommandCenter))]
        public int StreamerCommandCenterId { get; set; }
    }
}
