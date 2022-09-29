<template>
    <section>
        <div>
            <h4>List StreamerCommandCenter</h4>
            <b-alert :show="true" v-if="ApiCallMessage && ApiCallMessage.length >0">{{  ApiCallMessage }}</b-alert>
            <b-button-group>
                <b-button @click="Previous">Previous</b-button>
                <b-button @click="ListStreamerCommandCenter(DataModel,After)">{{After}}</b-button>
                <b-button @click="Next">Next</b-button>
            </b-button-group>
            <b-form></b-form>
            <b-table-simple>
                <b-thead>
                    <b-tr>
                        <b-th>StreamerId</b-th>
                        <b-th>CommandCenterId</b-th>
                        <b-th>Id</b-th>
                    </b-tr>
                </b-thead>
                <b-tbody>
                    <b-tr v-for=" a  of DataModel">
                        <b-td>{{ a.Item2.Name }}</b-td>
                        <b-td>{{ a.Item3.Name }}</b-td>
                        <b-button-group>
                            <b-button @click="Edit(a.Item1)">Edit</b-button>
                            <b-button @click="Delete(a.Item1)">Delete</b-button>
                        </b-button-group>
                        <b-td>{{ a.Item1.Id }}</b-td>
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
    import { ListStreamerCommandCenterRequest,  ListStreamerCommandCenterResponse, Tuple_3, StreamerCommandCenter, Streamer, CommandCenter } from '@/shared/dtos'


    export class StreamerCommandCenterListMask extends StreamerCommandCenter {




        Message: string = ""



        Success: boolean = true



        Completed: boolean = true



        Error: string = ""




        constructor(originalObject: StreamerCommandCenter) {

            super()
            Object.assign(this, originalObject)

        }

    }



    @Component({ components: {} })
    export class StreamerCommandCenterApiMixin extends Vue {




        ApiCallSuccess: boolean = true



        ApiCallMessage: string = ""




        async ListStreamerCommandCenter(DataModel: StreamerCommandCenterListMask[], After: number) {

            try {
                const Response: ListStreamerCommandCenterResponse = await client.get(new ListStreamerCommandCenterRequest({ After: After }))
                this.ApiCallSuccess = Response.Success;
                this.ApiCallMessage = Response.Message;
                if (Response.Success) {
                    return Response.StreamerCommandCenters;
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
    export class StreamerCommandCenterDataFields extends Mixins(StreamerCommandCenterApiMixin) {




        DataModel: Tuple_3<StreamerCommandCenter, Streamer, CommandCenter>[] = []



        After: number = 0




        async LoadStreamerCommandCenter() {

            this.DataModel = await this.ListStreamerCommandCenter([], this.After)

        }

    }



    @Component({ components: {} })
    export default class ListStreamerCommandCenter extends Mixins(StreamerCommandCenterDataFields) {






        async Previous() {

            this.LoadStreamerCommandCenter()

        }



        async Next() {

            this.LoadStreamerCommandCenter()

        }



        async created() {

            this.LoadStreamerCommandCenter()

        }



        async Delete() {

            this.$router.push('DeleteStreamerCommandCenter')

        }



        async Edit() {

            this.$router.push('EditStreamerCommandCenter')

        }

    }


</script>