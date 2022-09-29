using System;
using ServiceStack;
using ClipHost.ServiceModel;

namespace ClipHost.ServiceModel.ListStreamerCommandCenterModels
{


    public class ListStreamerCommandCenterRequest : IReturn<ListStreamerCommandCenterResponse>
    {


        public int After { get; set; }


    }
}
