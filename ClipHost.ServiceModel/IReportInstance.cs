using ClipHost.ServiceModel;
using System.Collections.Concurrent;
using BlazorQueue;

namespace ClipHost
{
    public interface IReportInstance
    {
       
       
        QueueReport[] ReportsArray { get; }

        DToProgramInstanceReport ToReport();
        void UpdateReport(QueueReport report);
    }
}