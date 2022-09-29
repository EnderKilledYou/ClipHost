<template>
    <section>
        <div>
            <h4>List CommandCenter</h4>
            <b-alert :show="true" v-if="ApiCallMessage && ApiCallMessage.length >0">{{  ApiCallMessage }}</b-alert>
            <b-button-group>
                <b-button @click="Previous">Previous</b-button>
                <b-button @click="ListCommandCenter(DataModel,After)">{{After}}</b-button>
                <b-button @click="Next">Next</b-button>
            </b-button-group>
            <b-form></b-form>
            <b-table-simple>
                <b-thead>
                    <b-tr>
                        <b-th>Name</b-th>
                        <b-th>StreamerCount</b-th>
                        <b-th>MaxStreamers</b-th>
                        <b-th>Id</b-th>
                    </b-tr>
                </b-thead>
                <b-tbody>
                    <b-tr v-for=" a  of DataModel">
                        <b-td>{{ a.Name }}</b-td>
                        <b-td>{{ a.StreamerCount }}</b-td>
                        <b-td>{{ a.MaxStreamers }}</b-td>
                        <b-button-group>
                            <b-button @click="Edit(a)">Edit</b-button>
                            <b-button @click="Delete(a)">Delete</b-button>
                        </b-button-group>
                        <b-td>{{ a.Id }}</b-td>
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
    import { ListCommandCenterRequest, CommandCenter, ListCommandCenterResponse } from '@/shared/dtos'


    export class CommandCenterListMask extends CommandCenter {




        Message: string = ""



        Success: boolean = true



        Completed: boolean = true



        Error: string = ""




        constructor(originalObject: CommandCenter) {

            super()
            Object.assign(this, originalObject)

        }

    }



    @Component({ components: {} })
    export class CommandCenterApiMixin extends Vue {




        ApiCallSuccess: boolean = true



        ApiCallMessage: string = ""




        async ListCommandCenter(DataModel: CommandCenterListMask[], After: number) {

            try {
                const Response: ListCommandCenterResponse = await client.get(new ListCommandCenterRequest({ After: After }))
                this.ApiCallSuccess = Response.Success;
                this.ApiCallMessage = Response.Message;
                if (Response.Success) {
                    return Response.CommandCenters.map(a => new CommandCenterListMask(a))
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
    export class CommandCenterDataFields extends Mixins(CommandCenterApiMixin) {




        DataModel: CommandCenterListMask[] = []



        After: number = 0




        async LoadCommandCenter() {

            this.DataModel = await this.ListCommandCenter(this.DataModel, this.After)

        }

    }



    @Component({ components: {} })
    export default class ListCommandCenter extends Mixins(CommandCenterDataFields) {






        async Previous() {

            this.LoadCommandCenter()

        }



        async Next() {

            this.LoadCommandCenter()

        }



        async created() {

            this.LoadCommandCenter()

        }



        async Delete() {

            this.$router.push('DeleteCommandCenter')

        }



        async Edit() {

            this.$router.push('EditCommandCenter')

        }

    }


</script>