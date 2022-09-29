using System;
using System.Collections.Generic;
using ClipHost.ServiceModel.Types;

namespace ClipHost.ServiceModel.ListClipFrameEventModels
{


    public class ListClipFrameEventResponse  {
                                    
                                
                                public long Count  {  get;  set; }  


                                
                                public string Message  {  get;  set; }  = "" ;


                                
                                public bool Success  {  get;  set; }  = true ;


                                
                                public List<ClipFrameEvent> ClipFrameEvents  {  get;  set; }  

                                    
                    }
}
