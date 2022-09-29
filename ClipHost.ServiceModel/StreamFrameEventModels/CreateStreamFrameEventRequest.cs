using System;
using ServiceStack;
using ClipHost.ServiceModel.Types;

namespace ClipHost.ServiceModel.CreateStreamFrameEventModels
{


    public class CreateStreamFrameEventRequest : IReturn<CreateStreamFrameEventResponse>
    {


        public StreamFrameEvent[] StreamFrameEvent { get; set; }


    }
}
