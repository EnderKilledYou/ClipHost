using System;
using ServiceStack;
using ClipHost.ServiceModel;

                    namespace ClipHost.ServiceModel.ListQueueReportModels {

                                 
                                 public class ListQueueReportRequest : IReturn<ListQueueReportResponse> {
                                    
                                
                                public int After  {  get;  set; }  


                                
                                public String Name  {  get;  set; }  

                                    
                    }
}
