using System;
using ServiceStack;
using ClipHost.ServiceModel.Types;

namespace ClipHost.ServiceModel.CreateFrameEventModels
{


    public class CreateFrameEventRequest : IReturn<CreateFrameEventResponse>
    {


        public StreamFrameEvent[] FrameEvent { get; set; }


    }
}
