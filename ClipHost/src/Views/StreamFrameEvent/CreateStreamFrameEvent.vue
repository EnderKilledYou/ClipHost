<template ><section ><div ><h4 >Create StreamFrameEventMask</h4>
<b-alert :show="true" v-if="DataModel.Message.length >0">{{  DataModel.Message }}</b-alert>
<b-form-group id="fieldset-TwitchStreamId" description="" label="TwitchStreamId" label-for="input-TwitchStreamId" valid-feedback=""><b-form-input id="fieldset-TwitchStreamId" :disabled="false" v-model="DataModel.TwitchStreamId" type="number" :trim="true"></b-form-input></b-form-group>
<b-form-group id="fieldset-FrameNumber" description="" label="FrameNumber" label-for="input-FrameNumber" valid-feedback=""><b-form-input id="fieldset-FrameNumber" :disabled="false" v-model="DataModel.FrameNumber" type="number" :trim="true"></b-form-input></b-form-group>
<b-form-group id="fieldset-FPS" description="" label="FPS" label-for="input-FPS" valid-feedback=""><b-form-input id="fieldset-FPS" :disabled="false" v-model="DataModel.FPS" type="number" :trim="true"></b-form-input></b-form-group>
<b-form-group id="fieldset-Second" description="" label="Second" label-for="input-Second" valid-feedback=""><b-form-input id="fieldset-Second" :disabled="false" v-model="DataModel.Second" type="number" :trim="true"></b-form-input></b-form-group>
<b-form-group id="fieldset-EventName" description="" label="EventName" label-for="input-EventName" valid-feedback=""><b-form-input id="fieldset-EventName" :disabled="false" v-model="DataModel.EventName" type="text" :trim="true"></b-form-input></b-form-group>
<b-button @click="CreateStreamFrameEvent(DataModel)">Create</b-button></div></section></template><script lang="ts">
                            console.log("");
                            import { Component,Vue } from 'vue-property-decorator'
import {Mixins}  from 'vue-property-decorator'
import {client}  from '@/shared'
import { CreateStreamFrameEventRequest,StreamFrameEvent,CreateStreamFrameEventResponse } from '@/shared/dtos'
                            
                                 
                                 export class StreamFrameEventCreateMask extends StreamFrameEvent {
                                    
                                    
                                    
                                
                                Message : string   = ""


                                
                                Success : boolean   = true


                                
                                Completed : boolean   = true


                                
                                Error : string   = ""

                                    
                                    
                          
                         constructor (init? : Partial<StreamFrameEventCreateMask>) {
                                    
                                     super()
;
 (Object as any).assign(this,init)
                                        
                                    }

                    }



                                 @Component({ components: {}})   
                                 export class StreamFrameEventApiMixin extends Vue {
                                    
                                    
                                    
                                    
                                    
                          
                        async CreateStreamFrameEvent (DataModel : StreamFrameEventCreateMask) {
                                    
                                    try {
                            const Response : CreateStreamFrameEventResponse = await client.post(new CreateStreamFrameEventRequest  ( { StreamFrameEvent : DataModel } ) )
 DataModel.Success = Response.Success;
if(Response.Id > 0)  {  DataModel.Message = 'Created' }  else {  DataModel.Message = Response.Message; } 
                            }catch(e : WebException){ 
                         DataModel.Message = e.message;
console.log(e)
const fieldErrors = e.GetFieldErrors()
if (fieldErrors){
}
                }finally{  
                        
}



                                        
                                    }

                    }



                                 @Component({ components: {}})   
                                 export default  class CreateStreamFrameEvent extends Mixins(StreamFrameEventApiMixin) {
                                    
                                    
                                    
                                
                                DataModel : StreamFrameEventCreateMask   = new StreamFrameEventCreateMask(new StreamFrameEvent({}))

                                    
                                    
                    }


                            </script>