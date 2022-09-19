using System;
using System.Collections.Generic;

namespace ClipHost.ServiceModel.ListDtoProgramInstanceModels
{


    public class ListDtoProgramInstanceResponse
    {


        public long Count { get; set; }



        public string Message { get; set; } = "";



        public bool Success { get; set; } = true;



        public DtoProgramInstance[] DtoProgramInstances { get; set; }


    }
}
