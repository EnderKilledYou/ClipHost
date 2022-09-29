using System;
using ServiceStack;
using ClipHost.ServiceModel.Types;

namespace ClipHost.ServiceModel.CreateStreamerCommandCenterModels
{


    public class CreateStreamerCommandCenterRequest : IReturn<CreateStreamerCommandCenterResponse>
    {


        public StreamerCommandCenter StreamerCommandCenter { get; set; }


    }
}
