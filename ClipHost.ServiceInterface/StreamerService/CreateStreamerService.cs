using ClipHost.ServiceModel.CreateStreamerModels;
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

                    namespace ClipHost.ServiceModel.CreateStreamerService {

                                 
                                 public class CreateStreamer : ServiceStack.Service {
                                    
                                    
                          
                      public  async Task<CreateStreamerResponse>  Post (CreateStreamerRequest request) {
                                    
                                    try {
                             var id= Db.Insert( request.Streamer,true);
return 
new CreateStreamerResponse() {  Id = id }
;
                            }catch(Exception e){ 
                        return 
new CreateStreamerResponse() {  Success = false
,Message = e.Message }
;
                }finally{  
                        
}



                                        
                                    }

                    }
}
