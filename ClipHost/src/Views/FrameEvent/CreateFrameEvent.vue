<template ><section ><div ><h4 >Create FrameEventMask</h4>
<b-alert :show="true" v-if="DataModel.Message.length >0">{{  DataModel.Message }}</b-alert>
<b-form-group id="fieldset-TwitchStreamId" description="" label="TwitchStreamId" label-for="input-TwitchStreamId" valid-feedback=""><b-form-input id="fieldset-TwitchStreamId" :disabled="false" v-model="DataModel.TwitchStreamId" type="number" :trim="true"></b-form-input></b-form-group>
<b-form-group id="fieldset-FrameNumber" description="" label="FrameNumber" label-for="input-FrameNumber" valid-feedback=""><b-form-input id="fieldset-FrameNumber" :disabled="false" v-model="DataModel.FrameNumber" type="number" :trim="true"></b-form-input></b-form-group>
<b-form-group id="fieldset-FPS" description="" label="FPS" label-for="input-FPS" valid-feedback=""><b-form-input id="fieldset-FPS" :disabled="false" v-model="DataModel.FPS" type="number" :trim="true"></b-form-input></b-form-group>
<b-form-group id="fieldset-Second" description="" label="Second" label-for="input-Second" valid-feedback=""><b-form-input id="fieldset-Second" :disabled="false" v-model="DataModel.Second" type="number" :trim="true"></b-form-input></b-form-group>
<b-form-group id="fieldset-EventName" description="" label="EventName" label-for="input-EventName" valid-feedback=""><b-form-input id="fieldset-EventName" :disabled="false" v-model="DataModel.EventName" type="text" :trim="true"></b-form-input></b-form-group>
<b-button @click="CreateFrameEvent(DataModel)">Create</b-button></div></section></template><script lang="ts">
                            console.log("");
                            import { Component,Vue } from 'vue-property-decorator'
import {Mixins}  from 'vue-property-decorator'
import {client}  from '@/shared'
import { CreateFrameEventRequest,FrameEvent,CreateFrameEventResponse } from '@/shared/dtos'
                            
                                 
                                 export class FrameEventCreateMask extends FrameEvent {
                                    
                                    
                                    
                                
                                Message : string   = ""


                                
                                Success : boolean   = true


                                
                                Completed : boolean   = true


                                
                                Error : string   = ""

                                    
                                    
                          
                         constructor (init? : Partial<FrameEventCreateMask>) {
                                    
                                     super()
;
 (Object as any).assign(this,init)
                                        
                                    }

                    }



                                 @Component({ components: {}})   
                                 export class FrameEventApiMixin extends Vue {
                                    
                                    
                                    
                                    
                                    
                          
                        async CreateFrameEvent (DataModel : FrameEventCreateMask) {
                                    
                                    try {
                            const Response : CreateFrameEventResponse = await client.post(new CreateFrameEventRequest  ( { FrameEvent : DataModel } ) )
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
                                 export default  class CreateFrameEvent extends Mixins(FrameEventApiMixin) {
                                    
                                    
                                    
                                
                                DataModel : FrameEventCreateMask   = new FrameEventCreateMask(new FrameEvent({}))

                                    
                                    
                    }


                            </script>