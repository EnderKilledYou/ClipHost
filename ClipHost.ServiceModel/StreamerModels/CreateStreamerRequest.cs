using System;
using ServiceStack;
using ClipHost.ServiceModel.Types;

namespace ClipHost.ServiceModel.CreateStreamerModels
{


    public class CreateStreamerRequest : IReturn<CreateStreamerResponse> {
                                    
                                
                                public Streamer Streamer  {  get;  set; }  

                                    
                    }
}
