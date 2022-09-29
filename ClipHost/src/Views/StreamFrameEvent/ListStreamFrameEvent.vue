<template ><section ><div ><h4 >List StreamFrameEvent</h4>
<b-alert :show="true" v-if="ApiCallMessage && ApiCallMessage.length >0">{{  ApiCallMessage }}</b-alert>
<b-button-group ><b-button @click="Previous">Previous</b-button>
<b-button @click="ListStreamFrameEvent(DataModel,After)">{{After}}</b-button>
<b-button @click="Next">Next</b-button></b-button-group>
<b-form ></b-form>
<b-table-simple ><b-thead ><b-tr ><b-th >TwitchStreamId</b-th>
<b-th >FrameNumber</b-th>
<b-th >FPS</b-th>
<b-th >Second</b-th>
<b-th >EventName</b-th>
<b-th >Id</b-th></b-tr></b-thead>
<b-tbody ><b-tr v-for=" a  of DataModel"><b-td >{{ a.TwitchStreamId }}</b-td>
<b-td >{{ a.FrameNumber }}</b-td>
<b-td >{{ a.FPS }}</b-td>
<b-td >{{ a.Second }}</b-td>
<b-td >{{ a.EventName }}</b-td>
<b-button-group ><b-button @click="Edit(a)">Edit</b-button>
<b-button @click="Delete(a)">Delete</b-button></b-button-group>
<b-td >{{ a.Id }}</b-td></b-tr></b-tbody></b-table-simple></div></section></template><script lang="ts">
                            console.log("");
                            import { Component,Vue } from 'vue-property-decorator'
import {Mixins}  from 'vue-property-decorator'
import {client}  from '@/shared'
import { ListStreamFrameEventRequest,StreamFrameEvent,ListStreamFrameEventResponse } from '@/shared/dtos'
                            
                                 
                                 export class StreamFrameEventListMask extends StreamFrameEvent {
                                    
                                    
                                    
                                
                                Message : string   = ""


                                
                                Success : boolean   = true


                                
                                Completed : boolean   = true


                                
                                Error : string   = ""

                                    
                                    
                          
                         constructor (originalObject : StreamFrameEvent) {
                                    
                                     super()
 Object.assign(this,originalObject)
                                        
                                    }

                    }



                                 @Component({ components: {}})   
                                 export class StreamFrameEventApiMixin extends Vue {
                                    
                                    
                                    
                                
                                ApiCallSuccess : boolean   = true


                                
                                ApiCallMessage : string   = ""

                                    
                                    
                          
                        async ListStreamFrameEvent (DataModel : StreamFrameEventListMask[],After : number) {
                                    
                                    try {
                            const Response : ListStreamFrameEventResponse = await client.get(new ListStreamFrameEventRequest({After : After}))
 this.ApiCallSuccess = Response.Success;
 this.ApiCallMessage = Response.Message;
 if ( Response.Success) {
return  Response.StreamFrameEvents.map(a => new StreamFrameEventListMask(a))
}
 return []
                            }catch(e : any){ 
                         this.ApiCallMessage = e.message;
console.log(e); return []
                }finally{  
                        
}



                                        
                                    }

                    }



                                 @Component({ components: {}})   
                                 export class StreamFrameEventDataFields extends Mixins(StreamFrameEventApiMixin) {
                                    
                                    
                                    
                                
                                DataModel : StreamFrameEventListMask[]   = []


                                
                                After : number   = 0

                                    
                                    
                          
                        async LoadStreamFrameEvent () {
                                    
                                     this.DataModel  = await this.ListStreamFrameEvent(this.DataModel,this.After)
                                        
                                    }

                    }



                                 @Component({ components: {}})   
                                 export default  class ListStreamFrameEvent extends Mixins(StreamFrameEventDataFields) {
                                    
                                    
                                    
                                    
                                    
                          
                        async Previous () {
                                    
                                     this.LoadStreamFrameEvent()
                                        
                                    }


                          
                        async Next () {
                                    
                                     this.LoadStreamFrameEvent()
                                        
                                    }


                          
                        async created () {
                                    
                                     this.LoadStreamFrameEvent()
                                        
                                    }


                          
                        async Delete () {
                                    
                                     this.$router.push('DeleteStreamFrameEvent')
                                        
                                    }


                          
                        async Edit () {
                                    
                                     this.$router.push('EditStreamFrameEvent')
                                        
                                    }

                    }


                            </script>