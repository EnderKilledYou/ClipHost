<template>
    <section>
        <div>
            <h4>Create ProcessReportMask</h4>
            <b-alert :show="true" v-if="DataModel.Message.length >0">{{  DataModel.Message }}</b-alert>
            <b-form-group id="fieldset-IsRunning" description="" label="IsRunning" label-for="input-IsRunning" valid-feedback="">
                <b-form-input id="fieldset-IsRunning" :disabled="false" v-model="DataModel.IsRunning" type="text" :trim="true"></b-form-input>
            </b-form-group>

            <b-form-group id="fieldset-ExitCode" description="" label="ExitCode" label-for="input-ExitCode" valid-feedback="">
                <b-form-input id="fieldset-ExitCode" :disabled="false" v-model="DataModel.ExitCode" type="number" :trim="true"></b-form-input>
            </b-form-group>

            <b-form-group id="fieldset-ReportText" description="" label="ReportText" label-for="input-ReportText" valid-feedback="">
                <b-form-input id="fieldset-ReportText" :disabled="false" v-model="DataModel.ReportText" type="text" :trim="true"></b-form-input>
            </b-form-group>

            <b-form-group id="fieldset-ProcessId" description="" label="ProcessId" label-for="input-ProcessId" valid-feedback="">
                <b-form-input id="fieldset-ProcessId" :disabled="false" v-model="DataModel.ProcessId" type="number" :trim="true"></b-form-input>
            </b-form-group>
            <b-form-group id="fieldset-StreamerCommandCenterId" description="" label="StreamerCommandCenterId" label-for="input-StreamerCommandCenterId" valid-feedback=""><b-form-input id="fieldset-StreamerCommandCenterId" :disabled="false" v-model="DataModel.StreamerCommandCenterId" type="number" :trim="true"></b-form-input></b-form-group>
            <b-button @click="CreateProcessReport(DataModel)">Create</b-button>
        </div>
    </section>
</template>
<script lang="ts">
    console.log("");
    import { Component, Vue } from 'vue-property-decorator'
    import { Mixins } from 'vue-property-decorator'
    import { client } from '@/shared'
    import { CreateProcessReportRequest, ProcessReport, CreateProcessReportResponse } from '@/shared/dtos'


    export class ProcessReportCreateMask extends ProcessReport {




        Message: string = ""



        Success: boolean = true



        Completed: boolean = true



        Error: string = ""




        constructor(init?: Partial<ProcessReportCreateMask>) {

            super()
                ;
            (Object as any).assign(this, init)

        }

    }



    @Component({ components: {} })
    export class ProcessReportApiMixin extends Vue {






        async CreateProcessReport(DataModel: ProcessReportCreateMask) {

            try {
                const Response: CreateProcessReportResponse = await client.post(new CreateProcessReportRequest({ ProcessReport: DataModel }))
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
    export default class CreateProcessReport extends Mixins(ProcessReportApiMixin) {




        DataModel: ProcessReportCreateMask = new ProcessReportCreateMask(new ProcessReport({}))



    }


</script>