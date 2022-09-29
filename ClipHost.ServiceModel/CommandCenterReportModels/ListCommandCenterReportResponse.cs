using System;
using System.Collections.Generic;
using ClipHost.ServiceModel.Types;

namespace ClipHost.ServiceModel.ListCommandCenterReportModels
{


    public class ListCommandCenterReportResponse  {
                                    
                                
                                public long Count  {  get;  set; }  


                                
                                public string Message  {  get;  set; }  = "" ;


                                
                                public bool Success  {  get;  set; }  = true ;


                                
                                public List<CommandCenterReport> CommandCenterReports  {  get;  set; }  

                                    
                    }
}
