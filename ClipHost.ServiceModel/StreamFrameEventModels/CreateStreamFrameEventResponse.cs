using ServiceStack;
using System;

namespace ClipHost.ServiceModel.CreateStreamFrameEventModels
{


    public class CreateStreamFrameEventResponse
    {


        public long Id { get; set; }



        public string Message { get; set; }



        public bool Success { get; set; } = true;



        public ResponseStatus ResponseStatus { get; set; }


    }
}
