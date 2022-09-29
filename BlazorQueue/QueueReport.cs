namespace BlazorQueue
{
    public class QueueReport : IQueueReport
    {
        public QueueReport(int id, int size, int maxSize, int averageSeconds, int highSeconds, int low, string name, int processId)
        {
            Id = id;
            Size = size;
            MaxSize = maxSize;
            AverageMilliSeconds = averageSeconds;
            HighMilliSeconds = highSeconds;
            LowMilliSeconds = low;
            Name = name;
            ProcessId = processId;
        }

        public int Id { get; }
        public int Size { get; set; }
        public int MaxSize { get; set; }

        public int AverageMilliSeconds { get; set; }

        public int HighMilliSeconds { get; set; }
        public int LowMilliSeconds { get; set; }

        public string Name { get; set; }

        public int ProcessId { get; } 
    }
}