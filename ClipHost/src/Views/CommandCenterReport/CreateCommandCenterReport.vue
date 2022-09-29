<template>
    <section>
        <div>
            <h4>Create CommandCenterReportMask</h4>
            <b-alert :show="true" v-if="DataModel.Message.length >0">{{  DataModel.Message }}</b-alert>
            <b-form-group id="fieldset-Name" description="" label="Name" label-for="input-Name" valid-feedback=""><b-form-input id="fieldset-Name" :disabled="false" v-model="DataModel.Name" type="text" :trim="true"></b-form-input></b-form-group>
            <b-form-group id="fieldset-TotalProcessed" description="" label="TotalProcessed" label-for="input-TotalProcessed" valid-feedback=""><b-form-input id="fieldset-TotalProcessed" :disabled="false" v-model="DataModel.TotalProcessed" type="number" :trim="true"></b-form-input></b-form-group>
            <b-form-group id="fieldset-AverageSeconds" description="" label="AverageSeconds" label-for="input-AverageSeconds" valid-feedback=""><b-form-input id="fieldset-AverageSeconds" :disabled="false" v-model="DataModel.AverageSeconds" type="number" :trim="true"></b-form-input></b-form-group>
            <b-form-group id="fieldset-HighSeconds" description="" label="HighSeconds" label-for="input-HighSeconds" valid-feedback=""><b-form-input id="fieldset-HighSeconds" :disabled="false" v-model="DataModel.HighSeconds" type="number" :trim="true"></b-form-input></b-form-group>
            <b-form-group id="fieldset-Low" description="" label="Low" label-for="input-Low" valid-feedback=""><b-form-input id="fieldset-Low" :disabled="false" v-model="DataModel.Low" type="number" :trim="true"></b-form-input></b-form-group>
            <b-form-group id="fieldset-MaxSize" description="" label="MaxSize" label-for="input-MaxSize" valid-feedback=""><b-form-input id="fieldset-MaxSize" :disabled="false" v-model="DataModel.MaxSize" type="number" :trim="true"></b-form-input></b-form-group>
            <b-form-group id="fieldset-_processId" description="" label="_processId" label-for="input-_processId" valid-feedback=""><b-form-input id="fieldset-_processId" :disabled="false" v-model="DataModel._processId" type="number" :trim="true"></b-form-input></b-form-group>
            <b-form-group id="fieldset-ProcessId" description="" label="ProcessId" label-for="input-ProcessId" valid-feedback=""><b-form-input id="fieldset-ProcessId" :disabled="false" v-model="DataModel.ProcessId" type="number" :trim="true"></b-form-input></b-form-group>
            <b-form-group id="fieldset-Size" description="" label="Size" label-for="input-Size" valid-feedback=""><b-form-input id="fieldset-Size" :disabled="false" v-model="DataModel.Size" type="number" :trim="true"></b-form-input></b-form-group>
            <b-form-group id="fieldset-StreamerCommandCenterId" description="" label="StreamerCommandCenterId" label-for="input-StreamerCommandCenterId" valid-feedback=""><b-form-input id="fieldset-StreamerCommandCenterId" :disabled="false" v-model="DataModel.StreamerCommandCenterId" type="number" :trim="true"></b-form-input></b-form-group>
            <b-button @click="CreateCommandCenterReport(DataModel)">Create</b-button>
        </div>
    </section>
</template>
<script lang="ts">
    console.log("");
    import { Component, Vue } from 'vue-property-decorator'
    import { Mixins } from 'vue-property-decorator'
    import { client } from '@/shared'
    import { CreateCommandCenterReportRequest, CommandCenterReport, CreateCommandCenterReportResponse } from '@/shared/dtos'


    export class CommandCenterReportCreateMask extends CommandCenterReport {




        Message: string = ""



        Success: boolean = true



        Completed: boolean = true



        Error: string = ""




        constructor(init?: Partial<CommandCenterReportCreateMask>) {

            super()
                ;
            (Object as any).assign(this, init)

        }

    }



    @Component({ components: {} })
    export class CommandCenterReportApiMixin extends Vue {

        async CreateCommandCenterReport(DataModel: CommandCenterReportCreateMask) {

            try {
                const Response: CreateCommandCenterReportResponse = await client.post(new CreateCommandCenterReportRequest({ CommandCenterReport: DataModel }))
                DataModel.Success = Response.Success;
                if (Response.Id > 0) { DataModel.Message = 'Created' } else { DataModel.Message = Response.Message; }
            } catch (e: any) {
                DataModel.Message = e.message;
                console.log(e)
                const fieldErrors = e.GetFieldErrors()
                if (fieldErrors) {
                }
            } finally {

            }




        }

    }



    @Component({ components: {} })
    export default class CreateCommandCenterReport extends Mixins(CommandCenterReportApiMixin) {




        DataModel: CommandCenterReportCreateMask = new CommandCenterReportCreateMask(new CommandCenterReport({}))



    }


</script>