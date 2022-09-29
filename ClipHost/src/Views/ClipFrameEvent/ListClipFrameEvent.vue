<template ><section ><div ><h4 >List ClipFrameEvent</h4>
<b-alert :show="true" v-if="ApiCallMessage && ApiCallMessage.length >0">{{  ApiCallMessage }}</b-alert>
<b-button-group ><b-button @click="Previous">Previous</b-button>
<b-button @click="ListClipFrameEvent(DataModel,After)">{{After}}</b-button>
<b-button @click="Next">Next</b-button></b-button-group>
<b-form ></b-form>
<b-table-simple ><b-thead ><b-tr ><b-th >TwitchClipId</b-th>
<b-th >FrameNumber</b-th>
<b-th >FPS</b-th>
<b-th >Second</b-th>
<b-th >EventName</b-th>
<b-th >Id</b-th></b-tr></b-thead>
<b-tbody ><b-tr v-for=" a  of DataModel"><b-td >{{ a.TwitchClipId }}</b-td>
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
import { ListClipFrameEventRequest,ClipFrameEvent,ListClipFrameEventResponse } from '@/shared/dtos'
                            
                                 
                                 export class ClipFrameEventListMask extends ClipFrameEvent {
                                    
                                    
                                    
                                
                                Message : string   = ""


                                
                                Success : boolean   = true


                                
                                Completed : boolean   = true


                                
                                Error : string   = ""

                                    
                                    
                          
                         constructor (originalObject : ClipFrameEvent) {
                                    
                                     super()
 Object.assign(this,originalObject)
                                        
                                    }

                    }



                                 @Component({ components: {}})   
                                 export class ClipFrameEventApiMixin extends Vue {
                                    
                                    
                                    
                                
                                ApiCallSuccess : boolean   = true


                                
                                ApiCallMessage : string   = ""

                                    
                                    
                          
                        async ListClipFrameEvent (DataModel : ClipFrameEventListMask[],After : number) {
                                    
                                    try {
                            const Response : ListClipFrameEventResponse = await client.get(new ListClipFrameEventRequest({After : After}))
 this.ApiCallSuccess = Response.Success;
 this.ApiCallMessage = Response.Message;
 if ( Response.Success) {
return  Response.ClipFrameEvents.map(a => new ClipFrameEventListMask(a))
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
                                 export class ClipFrameEventDataFields extends Mixins(ClipFrameEventApiMixin) {
                                    
                                    
                                    
                                
                                DataModel : ClipFrameEventListMask[]   = []


                                
                                After : number   = 0

                                    
                                    
                          
                        async LoadClipFrameEvent () {
                                    
                                     this.DataModel  = await this.ListClipFrameEvent(this.DataModel,this.After)
                                        
                                    }

                    }



                                 @Component({ components: {}})   
                                 export default  class ListClipFrameEvent extends Mixins(ClipFrameEventDataFields) {
                                    
                                    
                                    
                                    
                                    
                          
                        async Previous () {
                                    
                                     this.LoadClipFrameEvent()
                                        
                                    }


                          
                        async Next () {
                                    
                                     this.LoadClipFrameEvent()
                                        
                                    }


                          
                        async created () {
                                    
                                     this.LoadClipFrameEvent()
                                        
                                    }


                          
                        async Delete () {
                                    
                                     this.$router.push('DeleteClipFrameEvent')
                                        
                                    }


                          
                        async Edit () {
                                    
                                     this.$router.push('EditClipFrameEvent')
                                        
                                    }

                    }


                            </script>