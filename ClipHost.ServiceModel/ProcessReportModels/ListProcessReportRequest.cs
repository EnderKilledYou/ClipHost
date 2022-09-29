using System;
using ServiceStack;
using ClipHost.ServiceModel;

namespace ClipHost.ServiceModel.ListProcessReportModels
{


    public class ListProcessReportRequest : IReturn<ListProcessReportResponse>
    {


        public int After { get; set; }



        public bool? IsRunning { get; set; }



        public int ProcessId { get; set; }


    }
}
