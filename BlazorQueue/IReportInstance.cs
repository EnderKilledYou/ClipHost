namespace BlazorQueue
{
    public interface IReportInstance
    {


        QueueReport[] ReportsArray { get; }

        ProgramInstanceReport ToReport();
        void UpdateReport(QueueReport report);
    }
}