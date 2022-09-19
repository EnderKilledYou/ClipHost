using BlazorQueue;
using ClipHost.ServiceModel;
using Newtonsoft.Json;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace ClipHost;

public class DtoProgramInstance : ProgramInstance
{
    public int? DtoId { get; set; }
    [JsonIgnore]
    public ConcurrentDictionary<int, QueueReport> Reports { get; } = new();

    public QueueReport[] ReportsArray => Reports.Values.ToArray();
    public override DToProgramInstanceReport ToReport()
    {
        return new DToProgramInstanceReport(ConnectionId(), Process()?.Id ?? 0, ReportsArray);
    }
    public void UpdateReport(QueueReport report)
    {
        if (!Reports.ContainsKey(report.Id))
        {
            Reports[report.Id] = report;
            return;
        }
        Reports[report.Id] = report;
    }

}