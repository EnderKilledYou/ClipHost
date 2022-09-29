using ServiceStack.DataAnnotations;

namespace ClipHost.ServiceModel.Types
{
    [TableUp(4)]

    public class ProcessReport : TablesUp
    {
        [SearchField]
        public bool IsRunning { get; set; }
        public int ExitCode { get; set; }
        public string ReportText { get; set; }
        [SearchField]
        public int ProcessId { get; set; } = 0;

        [Required]
        [References(typeof(StreamerCommandCenter))]
        public int StreamerCommandCenterId { get; set; }
    }
}
