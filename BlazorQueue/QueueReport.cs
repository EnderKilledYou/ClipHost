namespace BlazorQueue
{
    public class QueueReport
    {
        public QueueReport(int id, int size, int maxSize, int averageSeconds, int highSeconds, int low, string name, int processId)
        {
            Id = id;
            Size = size;
            MaxSize = maxSize;
            AverageSeconds = averageSeconds;
            HighSeconds = highSeconds;
            Low = low;
            Name = name;
            ProcessId = processId;
        }

        public int Id { get; }
        public int Size { get; set; }
        public int MaxSize { get; set; }

        public int AverageSeconds { get; set; }

        public int HighSeconds { get; set; }
        public int Low { get; set; }
     
        public string Name { get; set; }
     
        public int ProcessId { get; }
    }
}