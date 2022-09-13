using ClipHost.ServiceModel.ListStreamerModels;
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

                    namespace ClipHost.ServiceModel.ListStreamerService {

                                 
                                 public class ListStreamer : Service {
                                    
                                    
                          
                      public  async Task<ListStreamerResponse>  Get (ListStreamerRequest request) {
                                    
                                    try {
                            
                var sqlStatement = Db.From<Streamer>().Where(a=>a.Id  > request.After);
                  
                

                sqlStatement= sqlStatement.Where(a=>a.Name.Contains(request.Name));

        var data = await Db.SelectAsync(sqlStatement);

return 
new ListStreamerResponse() {  Streamers = data
,Success = true
,Message = "" }
;
                            }catch(Exception e){ 
                        return 
new ListStreamerResponse() {  Success = false
,Message = e.Message }
;
                }finally{  
                        
}



                                        
                                    }

                    }
}
