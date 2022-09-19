using ServiceStack.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ClipHost.ServiceModel
{
    public interface ITableUp
    {
        public void TableUp(Action<Type[]> createTableIfNotExists);
    }
    public abstract class TablesUp : ITableUp
    {
        [PrimaryKey]
        [AutoIncrement]
        public int Id { get; set; }
        public void TableUp(Action<Type[]> createTableIfNotExists)
        {
            createTableIfNotExists(new Type[] { this.GetType() });
        }
    }

    [TableUp(1)]
    public class CommandCenter : TablesUp
    {

        public string Name { get; set; } = "";
        public int StreamerCount { get; set; } = 0;
        public int MaxStreamers { get; set; } = 0;


    }

    public class QueueReport
    {
        public QueueReport(int id,int size, int maxSize, int averageSeconds, int highSeconds, int low, string name, int processId)
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
        [SearchField]
        public string Name { get; set; }
        [PrimaryKey]
        public int ProcessId { get; }
    }
    [TableUp(1)]
    public class Streamer : TablesUp
    {
        [PrimaryKey]
        [AutoIncrement]
        public int Id { get; set; }

        [SearchField]
        [Required]
        public string Name { get; set; } = "";
    }
    [TableUp(2)]
    [CompositeKey("CommandCenterId", "StreamerId")]
    public class StreamerCommandCenter : TablesUp
    {
        [PrimaryKey]
        [AutoIncrement]
        public int Id { get; set; }

        [Required]
        [References(typeof(Streamer))]
        public int StreamerId { get; set; } = 0;

        [Required]
        [References(typeof(CommandCenter))]

        public int CommandCenterId { get; set; } = 0;
    }
    [TableUp(1)]
    public class MonitoredStreamType : TablesUp
    {
        [PrimaryKey]
        [AutoIncrement]
        public int Id { get; set; }
        [Required]
        public string StreamType { get; set; } = ""; //twitch, yt etc
    }

    [TableUp(2)]
    public class MonitoredVideoStream : TablesUp
    {
        [PrimaryKey]
        [AutoIncrement]
        public int Id { get; set; }
        public string ThumbNail { get; set; }
        [Required]
        public string StreamId { get; set; } //twitch stream id, yt etc
        [Required]
        [References(typeof(MonitoredStreamType))]
        public int MonitoredStreamTypeId { get; set; } = 0;
    }
    [TableUp(3)]

    [CompositeIndex("StreamerId", "MonitoredVideoStreamId")]
    public class StreamStatus : TablesUp
    {
        [PrimaryKey]
        [AutoIncrement]
        public int Id { get; set; }
        [Required]
        [References(typeof(Streamer))]
        public int StreamerId { get; set; } = 0;
        [Required]
        [References(typeof(MonitoredVideoStream))]
        public int MonitoredVideoStreamId { get; set; } = 0;

        [Required]
        public Dictionary<string, string> StatusValues { get; set; } = new Dictionary<string, string>();

    }
}
