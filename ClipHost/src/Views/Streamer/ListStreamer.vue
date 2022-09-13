<template ><section ><div ><h4 >List Streamer</h4>
<b-alert :show="true" v-if="ApiCallMessage.length >0">{{  ApiCallMessage }}</b-alert>
<b-button-group ><b-button @click="Previous">Previous</b-button>
<b-button @click="ListStreamer (DataModel,After,Name)">{{After}}</b-button>
<b-button @click="Next">Next</b-button></b-button-group>
<b-table-simple ><b-thead ><b-tr ><b-th >Id</b-th>
<b-th >Name</b-th></b-tr></b-thead>
<b-tbody ><b-tr v-for=" a  of DataModel"><b-button-group ><b-button @click="GotoEdit(a)">Edit</b-button>
<b-button @click="Delete(a)">Delete</b-button></b-button-group>
<b-th >{{ a.Id }}</b-th>
<b-th >{{ a.Name }}</b-th></b-tr></b-tbody></b-table-simple></div></section></template><script lang="ts">
                            console.log("");
                            import { Component,Vue } from 'vue-property-decorator'
import {Mixins}  from 'vue-property-decorator'
import {client}  from '@/shared'
import { ListStreamerRequest,Streamer,ListStreamerResponse } from '@/shared/dtos'
                            
                                 
                                 export class StreamerListMask extends Streamer {
                                    
                                    
                                    
                                
                                Message : string   = ""


                                
                                Success : boolean   = true


                                
                                Completed : boolean   = true


                                
                                Error : string   = ""

                                    
                                    
                          
                         constructor (originalObject : Streamer) {
                                    
                                     super()
 Object.assign(this,originalObject)
                                        
                                    }

                    }



                                 @Component({ components: {}})   
                                 export class StreamerApiMixin extends Vue {
                                    
                                    
                                    
                                
                                ApiCallSuccess : boolean   = true


                                
                                ApiCallMessage : string   = ""


                                
                                Name : string   = ""

                                    
                                    
                          
                        async ListStreamer (DataModel : StreamerListMask[],After : number,Name : string) {
                                    
                                    try {
                            const Response : ListStreamerResponse = await client.get(new ListStreamerRequest  ( { After : After } ) )
 this.ApiCallSuccess = Response.Success;
 this.ApiCallMessage = Response.Message;
 if ( Response.Success) {
 Response.Streamers.map(a => new StreamerListMask(a)).forEach(a=>DataModel.push(a))
}
                            }catch(e : any){ 
                         this.ApiCallMessage = e.message;
console.log(e)
                }finally{  
                        
}



                                        
                                    }

                    }



                                 @Component({ components: {}})   
                                 export default  class ListStreamer extends Mixins(StreamerApiMixin) {
                                    
                                    
                                    
                                
                                DataModel : StreamerListMask[]   = []


                                
                                After : number   = 0

                                    
                                    
                    }


                            </script>