<template>
    <section>
        <div>
            <h4>List CommandCenterReport</h4>
            <b-alert :show="true" v-if="ApiCallMessage && ApiCallMessage.length >0">{{  ApiCallMessage }}</b-alert>
            <b-button-group>
                <b-button @click="Previous">Previous</b-button>
                <b-button @click="ListCommandCenterReport(DataModel,After)">{{After}}</b-button>
                <b-button @click="Next">Next</b-button>
            </b-button-group>
            <b-form></b-form>
            <b-table-simple>
                <b-thead>
                    <b-tr>
                        <b-th>Name</b-th>
                        <b-th>TotalProcessed</b-th>
                        <b-th>AverageSeconds</b-th>
                        <b-th>HighSeconds</b-th>
                        <b-th>Low</b-th>
                        <b-th>MaxSize</b-th>
                        <b-th>ProcessId</b-th>
                        <b-th>Size</b-th>

                    </b-tr>
                </b-thead>
                <b-tbody>
                    <b-tr v-for=" a  of DataModel">
                        <b-td>{{ a.Name }}</b-td>
                        <b-td>{{ a.TotalProcessed }}</b-td>
                        <b-td>{{ a.AverageMilliSeconds }}</b-td>
                        <b-td>{{ a.HighMilliSeconds }}</b-td>
                        <b-td>{{ a.LowMilliSeconds }}</b-td>
                        <b-td>{{ a.MaxSize }}</b-td>
                        <b-td>{{ a.ProcessId }}</b-td>
                        <b-td>{{ a.Size }}</b-td>
                        <b-td>
                            <b-progress :value="a.Size" :max=" a.MaxSize" show-progress animated></b-progress>
                        </b-td>

                    </b-tr>
                </b-tbody>
            </b-table-simple>
        </div>
    </section>
</template>
<script lang="ts">
    console.log("");
    import { Component, Vue } from 'vue-property-decorator'
    import { Mixins } from 'vue-property-decorator'
    import { client } from '@/shared'
    import { ListCommandCenterReportRequest, CommandCenterReport, ListCommandCenterReportResponse } from '@/shared/dtos'


    export class CommandCenterReportListMask extends CommandCenterReport {




        Message: string = ""



        Success: boolean = true



        Completed: boolean = true



        Error: string = ""




        constructor(originalObject: CommandCenterReport) {

            super()
            Object.assign(this, originalObject)

        }

    }



    @Component({ components: {} })
    export class CommandCenterReportApiMixin extends Vue {




        ApiCallSuccess: boolean = true



        ApiCallMessage: string = ""




        async ListCommandCenterReport(DataModel: CommandCenterReportListMask[], After: number) {

            try {
                const Response: ListCommandCenterReportResponse = await client.get(new ListCommandCenterReportRequest({ After: After }))
                this.ApiCallSuccess = Response.Success;
                this.ApiCallMessage = Response.Message;
                if (Response.Success) {
                    return Response.CommandCenterReports.map(a => new CommandCenterReportListMask(a))
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
    export class CommandCenterReportDataFields extends Mixins(CommandCenterReportApiMixin) {




        DataModel: CommandCenterReportListMask[] = []



        After: number = 0




        async LoadCommandCenterReport() {

            this.DataModel = await this.ListCommandCenterReport(this.DataModel, this.After)

        }

    }



    @Component({ components: {} })
    export default class ListCommandCenterReport extends Mixins(CommandCenterReportDataFields) {






        async Previous() {

            this.LoadCommandCenterReport()

        }



        async Next() {

            this.LoadCommandCenterReport()

        }



        async created() {

            this.LoadCommandCenterReport()

        }



        async Delete() {

            this.$router.push('DeleteCommandCenterReport')

        }



        async Edit() {

            this.$router.push('EditCommandCenterReport')

        }

    }


</script>