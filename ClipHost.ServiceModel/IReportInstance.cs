using ClipHost.ServiceModel;
using System.Collections.Concurrent;

namespace ClipHost
{
    public interface IReportInstance
    {
       
       
        QueueReport[] ReportsArray { get; }

        DToProgramInstanceReport ToReport();
        void UpdateReport(QueueReport report);
    }
}