using System;
using ServiceStack;
using ClipHost.ServiceModel;

                    namespace ClipHost.ServiceModel.ListStreamerModels {

                                 
                                 public class ListStreamerRequest : IReturn<ListStreamerResponse> {
                                    
                                
                                public int After  {  get;  set; }  


                                
                                public String Name  {  get;  set; }  

                                    
                    }
}
