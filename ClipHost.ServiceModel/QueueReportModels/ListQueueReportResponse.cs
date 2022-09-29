using System;
using System.Collections.Generic;
using BlazorQueue;

namespace ClipHost.ServiceModel.ListQueueReportModels
{
    public class ListQueueReportResponse
    {
        public long Count { get; set; }


        public string Message { get; set; } = "";


        public bool Success { get; set; } = true;


        public List<QueueReport> QueueReports { get; set; }
    }
}