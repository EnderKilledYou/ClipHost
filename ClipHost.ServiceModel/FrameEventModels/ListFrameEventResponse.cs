using System;
using System.Collections.Generic;
using ClipHost.ServiceModel.Types;

namespace ClipHost.ServiceModel.ListFrameEventModels
{


    public class ListFrameEventResponse  {
                                    
                                
                                public long Count  {  get;  set; }  


                                
                                public string Message  {  get;  set; }  = "" ;


                                
                                public bool Success  {  get;  set; }  = true ;


                                
                                public List<StreamFrameEvent> FrameEvents  {  get;  set; }  

                                    
                    }
}
