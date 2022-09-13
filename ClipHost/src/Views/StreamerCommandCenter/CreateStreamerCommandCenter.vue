<template ><section ><div ><h4 >Create StreamerCommandCenterMask</h4>
<b-alert :show="true" v-if="DataModel.Message.length >0">{{  DataModel.Message }}</b-alert>
<b-form-group id="fieldset-StreamerId" description="" label="StreamerId" label-for="input-StreamerId" valid-feedback="">
    <b-form-input id="fieldset-StreamerId" :disabled="false" v-model="DataModel.StreamerId" type="number" :trim="true"></b-form-input></b-form-group>
<b-form-group id="fieldset-CommandCenterId" description="" label="CommandCenterId" label-for="input-CommandCenterId" valid-feedback=""><b-form-input id="fieldset-CommandCenterId" :disabled="false" v-model="DataModel.CommandCenterId" type="number" :trim="true"></b-form-input></b-form-group>
<b-button @click="CreateStreamerCommandCenter(DataModel)">Create</b-button></div></section></template><script lang="ts">
    console.log("");
    import { Component, Vue } from 'vue-property-decorator'
    import { Mixins } from 'vue-property-decorator'
    import { client } from '@/shared'
    import { CreateStreamerCommandCenterRequest, StreamerCommandCenter, CreateStreamerCommandCenterResponse } from '@/shared/dtos'


    export class StreamerCommandCenterCreateMask extends StreamerCommandCenter {




        Message: string = ""



        Success: boolean = true



        Completed: boolean = true



        Error: string = ""




        constructor(init?: Partial<StreamerCommandCenterCreateMask>) {

            super()
                ;
            (Object as any).assign(this, init)

        }

    }



    @Component({ components: {} })
    export class StreamerCommandCenterApiMixin extends Vue {






        async CreateStreamerCommandCenter(DataModel: StreamerCommandCenterCreateMask) {

            try {
                const Response: CreateStreamerCommandCenterResponse = await client.post(new CreateStreamerCommandCenterRequest({ StreamerCommandCenter: DataModel }))
                DataModel.Success = Response.Success;
                if (Response.Id > 0) { DataModel.Message = 'Created' } else { DataModel.Message = Response.Message; }
            } catch (e: any) {
                DataModel.Message = e.message;
                console.log(e)
            } finally {

            }




        }

    }



    @Component({ components: {} })
    export default class CreateStreamerCommandCenter extends Mixins(StreamerCommandCenterApiMixin) {




        DataModel: StreamerCommandCenterCreateMask = new StreamerCommandCenterCreateMask(new StreamerCommandCenter({}))



    }


</script>