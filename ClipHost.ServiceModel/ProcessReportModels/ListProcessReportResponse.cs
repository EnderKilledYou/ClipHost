using System;
using System.Collections.Generic;
using ClipHost.ServiceModel.Types;

namespace ClipHost.ServiceModel.ListProcessReportModels
{


    public class ListProcessReportResponse
    {


        public long Count { get; set; }



        public string Message { get; set; } = "";



        public bool Success { get; set; } = true;
        public List<Tuple<StreamerCommandCenter, ProcessReport, Streamer, CommandCenter>> ProcessReports { get; set; }
    }
}
