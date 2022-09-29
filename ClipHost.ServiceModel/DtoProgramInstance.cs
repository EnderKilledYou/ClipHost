using BlazorQueue;
using ClipHost.ServiceModel;
using Newtonsoft.Json;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace ClipHost;

public class DtoProgramInstance : ProgramInstance, IDtoProgramInstance
{
    public int? DtoId { get; set; }

    private readonly ConcurrentDictionary<int, QueueReport> Reports = new();

 
    public override QueueReport[] ReportsArray => Reports.Values.ToArray();

    public int DatabaseId { get; set; }

    public override DToProgramInstanceReport ToReport()
    {
        return new DToProgramInstanceReport(ConnectionId(), Process()?.Id ?? 0, ReportsArray);
    }
    public override void UpdateReport(QueueReport report)
    {
        if (!Reports.ContainsKey(report.Id))
        {
            Reports[report.Id] = report;
            return;
        }
        Reports[report.Id] = report;
    }

}