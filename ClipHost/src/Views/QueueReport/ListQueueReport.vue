<template>
    <section>
        <div>
            <h4>List QueueReport</h4>
            <b-alert :show="true" v-if="ApiCallMessage && ApiCallMessage.length >0">{{  ApiCallMessage }}</b-alert>
            <b-button-group>
                <b-button @click="Previous">Previous</b-button>
                <b-button @click="ListQueueReport (DataModel,After,Name)">{{After}}</b-button>
                <b-button @click="Next">Next</b-button>
            </b-button-group>
            <b-form>
                <label class="sr-only" for="NameId">Name</label>
                <b-form-input v-model="Name" id="NameId"></b-form-input>
            </b-form>
            <b-table-simple>
                <b-thead>
                    <b-tr>
                        <b-th>Id</b-th>
                        <b-th>Size</b-th>
                        <b-th>MaxSize</b-th>
                        <b-th>AverageSeconds</b-th>
                        <b-th>HighSeconds</b-th>
                        <b-th>Low</b-th>
                        <b-th>Name</b-th>
                        <b-th>ProcessId</b-th>
                    </b-tr>
                </b-thead>
                <b-tbody>
                    <b-tr v-for=" a  of DataModel" :key="a.Id">
                        <b-td>{{ a.Id }}</b-td>
                        <b-td>

                            <b-progress  :value="a.Size" :max=" a.MaxSize" show-progress animated></b-progress>
                        </b-td>
                        <b-td>{{ a.MaxSize }}</b-td>
                        <b-td>{{ a.AverageSeconds }}</b-td>
                        <b-td>{{ a.HighSeconds }}</b-td>
                        <b-td>{{ a.Low }}</b-td>
                        <b-td>{{ a.Name }}</b-td>
                        <b-button-group>
                            <b-button @click="Edit(a)">Edit</b-button>
                            <b-button @click="Delete(a)">Delete</b-button>
                        </b-button-group>
                        <b-td>{{ a.ProcessId }}</b-td>
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
    import { ListQueueReportRequest, QueueReport, ListQueueReportResponse } from '@/shared/dtos'


    export class QueueReportListMask extends QueueReport {




        Message: string = ""



        Success: boolean = true



        Completed: boolean = true



        Error: string = ""




        constructor(originalObject: QueueReport) {

            super()
            Object.assign(this, originalObject)

        }

    }



    @Component({ components: {} })
    export class QueueReportApiMixin extends Vue {




        ApiCallSuccess: boolean = true



        ApiCallMessage: string = ""




        async ListQueueReport(DataModel: QueueReportListMask[], After: number, Name: string) {

            try {
                const Response: ListQueueReportResponse = await client.get(new ListQueueReportRequest({
                    After: After,
                    Name: Name
                }))
                this.ApiCallSuccess = Response.Success;
                this.ApiCallMessage = Response.Message;
                if (Response.Success) {
                    return Response.QueueReports.map(a => new QueueReportListMask(a))
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
    export class QueueReportDataFields extends Mixins(QueueReportApiMixin) {




        DataModel: QueueReportListMask[] = []



        After: number = 0



        Name: string = ""




        async LoadQueueReport() {

            this.DataModel = await this.ListQueueReport(this.DataModel, this.After, this.Name)

        }

    }



    @Component({ components: {} })
    export default class ListQueueReport extends Mixins(QueueReportDataFields) {






        async Previous() {

            this.LoadQueueReport()

        }



        async Next() {

            this.LoadQueueReport()

        }



        async created() {

            this.LoadQueueReport()

        }



        async Delete() {

            this.$router.push('DeleteQueueReport')

        }



        async Edit() {

            this.$router.push('EditQueueReport')

        }

    }


</script>