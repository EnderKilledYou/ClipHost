using System;
using System.Collections.Generic;
using ClipHost.ServiceModel.Types;

namespace ClipHost.ServiceModel.ListCommandCenterModels
{


    public class ListCommandCenterResponse  {
                                    
                                
                                public long Count  {  get;  set; }  


                                
                                public string Message  {  get;  set; }  = "" ;


                                
                                public bool Success  {  get;  set; }  = true ;


                                
                                public List<CommandCenter> CommandCenters  {  get;  set; }  

                                    
                    }
}
