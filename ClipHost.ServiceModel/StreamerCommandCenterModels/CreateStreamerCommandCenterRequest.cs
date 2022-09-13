using System;
using ServiceStack;
using ClipHost.ServiceModel;

namespace ClipHost.ServiceModel.CreateStreamerCommandCenterModels
{


    public class CreateStreamerCommandCenterRequest : IReturn<CreateStreamerCommandCenterResponse>
    {


        public StreamerCommandCenter StreamerCommandCenter { get; set; }


    }
}
