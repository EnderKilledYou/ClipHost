using System;
using System.Collections.Generic;

                    namespace ClipHost.ServiceModel.ListStreamerModels {

                                 
                                 public class ListStreamerResponse  {
                                    
                                
                                public long Count  {  get;  set; }  


                                
                                public string Message  {  get;  set; }  = "" ;


                                
                                public bool Success  {  get;  set; }  = true ;


                                
                                public List<Streamer> Streamers  {  get;  set; }  

                                    
                    }
}
