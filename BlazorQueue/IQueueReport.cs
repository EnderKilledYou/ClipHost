namespace BlazorQueue
{
    public interface IQueueReport
    {
        int AverageMilliSeconds { get; set; }
        int HighMilliSeconds { get; set; }
        int Id { get; }
        int LowMilliSeconds { get; set; }
        int MaxSize { get; set; }
        string Name { get; set; }
        int ProcessId { get; }
        int Size { get; set; }
    }
}