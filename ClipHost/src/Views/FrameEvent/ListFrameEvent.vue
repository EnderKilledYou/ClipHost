<template ><section ><div ><h4 >List FrameEvent</h4>
<b-alert :show="true" v-if="ApiCallMessage && ApiCallMessage.length >0">{{  ApiCallMessage }}</b-alert>
<b-button-group ><b-button @click="Previous">Previous</b-button>
<b-button @click="ListFrameEvent(DataModel,After)">{{After}}</b-button>
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
import { ListFrameEventRequest,FrameEvent,ListFrameEventResponse } from '@/shared/dtos'
                            
                                 
                                 export class FrameEventListMask extends FrameEvent {
                                    
                                    
                                    
                                
                                Message : string   = ""


                                
                                Success : boolean   = true


                                
                                Completed : boolean   = true


                                
                                Error : string   = ""

                                    
                                    
                          
                         constructor (originalObject : FrameEvent) {
                                    
                                     super()
 Object.assign(this,originalObject)
                                        
                                    }

                    }



                                 @Component({ components: {}})   
                                 export class FrameEventApiMixin extends Vue {
                                    
                                    
                                    
                                
                                ApiCallSuccess : boolean   = true


                                
                                ApiCallMessage : string   = ""

                                    
                                    
                          
                        async ListFrameEvent (DataModel : FrameEventListMask[],After : number) {
                                    
                                    try {
                            const Response : ListFrameEventResponse = await client.get(new ListFrameEventRequest({After : After}))
 this.ApiCallSuccess = Response.Success;
 this.ApiCallMessage = Response.Message;
 if ( Response.Success) {
return  Response.FrameEvents.map(a => new FrameEventListMask(a))
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
                                 export class FrameEventDataFields extends Mixins(FrameEventApiMixin) {
                                    
                                    
                                    
                                
                                DataModel : FrameEventListMask[]   = []


                                
                                After : number   = 0

                                    
                                    
                          
                        async LoadFrameEvent () {
                                    
                                     this.DataModel  = await this.ListFrameEvent(this.DataModel,this.After)
                                        
                                    }

                    }



                                 @Component({ components: {}})   
                                 export default  class ListFrameEvent extends Mixins(FrameEventDataFields) {
                                    
                                    
                                    
                                    
                                    
                          
                        async Previous () {
                                    
                                     this.LoadFrameEvent()
                                        
                                    }


                          
                        async Next () {
                                    
                                     this.LoadFrameEvent()
                                        
                                    }


                          
                        async created () {
                                    
                                     this.LoadFrameEvent()
                                        
                                    }


                          
                        async Delete () {
                                    
                                     this.$router.push('DeleteFrameEvent')
                                        
                                    }


                          
                        async Edit () {
                                    
                                     this.$router.push('EditFrameEvent')
                                        
                                    }

                    }


                            </script>