using System;
using ServiceStack;
using ClipHost.ServiceModel.Types;

namespace ClipHost.ServiceModel.CreateCommandCenterReportModels
{


    public class CreateCommandCenterReportRequest : IReturn<CreateCommandCenterReportResponse> {
                                    
                                
                                public CommandCenterReport CommandCenterReport  {  get;  set; }  

                                    
                    }
}
