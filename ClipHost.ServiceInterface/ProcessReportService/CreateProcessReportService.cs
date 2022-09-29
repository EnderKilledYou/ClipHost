using ClipHost.ServiceModel.CreateProcessReportModels;
using Microsoft.AspNetCore.Hosting;
using ServiceStack;
using ServiceStack.Auth;
using ServiceStack.Configuration;
using ServiceStack.Logging;
using System.Linq;
using ServiceStack.OrmLite;
using ServiceStack.FluentValidation;
using System;
using System.Threading.Tasks;

                    namespace ClipHost.ServiceModel.CreateProcessReportService {

                                 
                                 public class CreateProcessReport : ServiceStack.Service {
                                    
                                    
                          
                      public  async Task<CreateProcessReportResponse>  Post (CreateProcessReportRequest request) {
                                    
                                    try {
                             var id= Db.Insert( request.ProcessReport,true);
return 
new CreateProcessReportResponse() {  Id = id }
;
                            }catch(Exception e){ 
                        return 
new CreateProcessReportResponse() {  Success = false
,Message = e.Message }
;
                }finally{  
                        
}



                                        
                                    }

                    }
}
