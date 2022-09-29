using System;
using System.Collections.Generic;
using ClipHost.ServiceModel.Types;

namespace ClipHost.ServiceModel.ListStreamerCommandCenterModels
{


    public class ListStreamerCommandCenterResponse
    {


        public long Count { get; set; }



        public string Message { get; set; } = "";



        public bool Success { get; set; } = true;
        public List<Tuple<StreamerCommandCenter, Streamer, CommandCenter>> StreamerCommandCenters { get; set; }
    }
}
