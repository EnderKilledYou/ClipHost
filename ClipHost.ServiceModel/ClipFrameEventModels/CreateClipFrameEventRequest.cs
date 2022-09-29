using System;
using ServiceStack;
using ClipHost.ServiceModel.Types;

namespace ClipHost.ServiceModel.CreateClipFrameEventModels
{


    public class CreateClipFrameEventRequest : IReturn<CreateClipFrameEventResponse>
    {


        public ClipFrameEvent[] ClipFrameEvent { get; set; }


    }
}
