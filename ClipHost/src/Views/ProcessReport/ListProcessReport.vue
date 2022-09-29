<template>
    <section>
        <div>
            <h4>List ProcessReport</h4>
            <b-alert :show="true" v-if="ApiCallMessage && ApiCallMessage.length >0">{{  ApiCallMessage }}</b-alert>
            <b-button-group>
                <b-button @click="Previous">Previous</b-button>
                <b-button @click="ListProcessReport (DataModel,After,IsRunning,ProcessId)">{{After}}</b-button>
                <b-button @click="Next">Next</b-button>
            </b-button-group>
            <b-form>
                <label class="sr-only" for="IsRunningId">IsRunning</label>
                <b-checkbox :checked="IsRunning" id="IsRunningId"></b-checkbox>
                <label class="sr-only" for="ProcessIdId">ProcessId</label>
                <b-form-input v-model="ProcessId" id="ProcessIdId"></b-form-input>
            </b-form>
            <b-table-simple>
                <b-thead>
                    <b-tr>
                        <b-th>IsRunning</b-th>
                        <b-th>ExitCode</b-th>
                        <b-th>ReportText</b-th>
                        <b-th>ProcessId</b-th>
                        <b-th>Streamer</b-th>

                    </b-tr>
                </b-thead>
                <b-tbody>
                    <b-tr v-for=" a  of DataModel">
                        <b-td>{{ a.Item1.IsRunning }}</b-td>
                        <b-td>{{ a.Item1.ExitCode }}</b-td>
                        <b-td>{{ a.Item1.ReportText }}</b-td>
                        <b-td>{{ a.Item1.ProcessId }}</b-td>
                        <b-td>{{ a.Item3?.Name}}</b-td>
                        <b-button-group>
                            <b-button @click="Edit(a)">Edit</b-button>
                            <b-button @click="Delete(a)">Delete</b-button>
                        </b-button-group>

                    </b-tr>
                </b-tbody>
            </b-table-simple>
        </div>
    </section>
</template>
<script lang="ts">
    console.log("");
    import { Component, Vue } from 'vue-property-decorator'
    import { Mixins, Watch } from 'vue-property-decorator'
    import { client } from '@/shared'
    import { ListProcessReportRequest, ProcessReport, ListProcessReportResponse, Streamer, CommandCenter, StreamerCommandCenter, Tuple_4 } from '@/shared/dtos'


    export class ProcessReportListMask extends ProcessReport {




        Message: string = ""



        Success: boolean = true



        Completed: boolean = true



        Error: string = ""




        constructor(originalObject: ProcessReport) {

            super()
            Object.assign(this, originalObject)

        }

    }



    @Component({ components: {} })
    export class ProcessReportApiMixin extends Vue {




        ApiCallSuccess: boolean = true



        ApiCallMessage: string = ""




        async ListProcessReport(DataModel: ProcessReportListMask[], After: number, IsRunning: boolean, ProcessId: number) {

            try {
                const Response: ListProcessReportResponse = await client.get(new ListProcessReportRequest({
                    After: After,
                    IsRunning: IsRunning,
                    ProcessId: ProcessId
                }))
                this.ApiCallSuccess = Response.Success;
                this.ApiCallMessage = Response.Message;
                if (Response.Success) {
                    return Response.ProcessReports;
                }
                return []
            } catch (e: any) {
                this.ApiCallMessage = e.message;
                console.log(e); return []
            } finally {

            }




        }

    }



    @Component({ components: {} })
    export class ProcessReportDataFields extends Mixins(ProcessReportApiMixin) {




        DataModel: Tuple_3<ProcessReport, StreamerCommandCenter, Streamer>[] = []



        After: number = 0



        IsRunning: boolean = true
        @Watch('IsRunning') OnFilterChange(a: boolean, b: boolean) {
            this.LoadProcessReport();
        }


        ProcessId: number = 0




        async LoadProcessReport() {

            this.DataModel = await this.ListProcessReport(this.DataModel, this.After, this.IsRunning, this.ProcessId)

        }

    }



    @Component({ components: {} })
    export default class ListProcessReport extends Mixins(ProcessReportDataFields) {






        async Previous() {

            this.LoadProcessReport()

        }



        async Next() {

            this.LoadProcessReport()

        }



        async created() {

            this.LoadProcessReport()

        }



        async Delete() {

            this.$router.push('DeleteProcessReport')

        }



        async Edit() {

            this.$router.push('EditProcessReport')

        }

    }


</script>