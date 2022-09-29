using ServiceStack;
using System;

namespace ClipHost.ServiceModel.CreateProcessReportModels
{


    public class CreateProcessReportResponse
    {


        public long Id { get; set; }



        public string Message { get; set; }



        public bool Success { get; set; } = true;



        public ResponseStatus ResponseStatus { get; set; }


    }
}
