using System;
using ServiceStack;
using ClipHost.ServiceModel.Types;

namespace ClipHost.ServiceModel.CreateProcessReportModels
{


    public class CreateProcessReportRequest : IReturn<CreateProcessReportResponse> {
                                    
                                
                                public ProcessReport ProcessReport  {  get;  set; }  

                                    
                    }
}
