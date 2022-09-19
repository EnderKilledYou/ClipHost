using BlazorQueue;
using ClipHost.ServiceModel;

namespace ClipHost
{
    public class DToProgramInstanceReport : ProgramInstanceReport
    {
        public QueueReport[] QueueReports { get; }

        public int DtoId { get; set; }

        public DToProgramInstanceReport(string connectionId, int processId) : base(connectionId, processId)
        {
        }

        public DToProgramInstanceReport(string connectionId, int processId, QueueReport[] queueReports) : this(connectionId, processId)
        {
            this.QueueReports = queueReports;
        }
    }

    public class StreamerCommandCenterDToProgramInstanceReport : DToProgramInstanceReport
    {
        public StreamerCommandCenterDToProgramInstanceReport(string connectionId, int processId) : base(connectionId, processId)
        {
        }
    }
}