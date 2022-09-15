<template ><section ><div ><h4 >List Streamer</h4>
<b-alert :show="true" v-if="ApiCallMessage && ApiCallMessage.length >0">{{  ApiCallMessage }}</b-alert>
<b-button-group ><b-button @click="Previous">Previous</b-button>
<b-button @click="ListStreamer (DataModel,After,Name)">{{After}}</b-button>
<b-button @click="Next">Next</b-button></b-button-group>
<b-form ><label class="sr-only" for="NameId">Name</label>
<b-form-input v-model="Name" id="NameId"></b-form-input></b-form>
<b-table-simple ><b-thead ><b-tr ><b-th >Id</b-th>
<b-th >Name</b-th></b-tr></b-thead>
<b-tbody ><b-tr v-for=" a  of DataModel"><b-button-group ><b-button @click="Edit(a)">Edit</b-button>
<b-button @click="Delete(a)">Delete</b-button></b-button-group>
<b-td >{{ a.Id }}</b-td>
<b-td >{{ a.Name }}</b-td></b-tr></b-tbody></b-table-simple></div></section></template><script lang="ts">
    console.log("");
    import { Component, Vue } from 'vue-property-decorator'
    import { Mixins } from 'vue-property-decorator'
    import { client } from '@/shared'
    import { ListStreamerRequest, Streamer, ListStreamerResponse } from '@/shared/dtos'


    export class StreamerListMask extends Streamer {




        Message: string = ""



        Success: boolean = true



        Completed: boolean = true



        Error: string = ""




        constructor(originalObject: Streamer) {

            super()
            Object.assign(this, originalObject)

        }

    }



    @Component({ components: {} })
    export class StreamerApiMixin extends Vue {




        ApiCallSuccess: boolean = true



        ApiCallMessage: string = ""




        async ListStreamer(DataModel: StreamerListMask[], After: number, Name: string) {

            try {
                const Response: ListStreamerResponse = await client.get(new ListStreamerRequest({
                    After: After,
                    Name: Name
                }))
                this.ApiCallSuccess = Response.Success;
                this.ApiCallMessage = Response.Message;
                if (Response.Success) {
                    return Response.Streamers.map(a => new StreamerListMask(a))
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
    export class StreamerDataFields extends Mixins(StreamerApiMixin) {




        DataModel: StreamerListMask[] = []



        After: number = 0



        Name: string = ""




        async LoadStreamer() {

            this.DataModel = await this.ListStreamer(this.DataModel, this.After, this.Name)

        }

    }



    @Component({ components: {} })
    export default class ListStreamer extends Mixins(StreamerDataFields) {






        async Previous() {

            this.LoadStreamer()

        }



        async Next() {

            this.LoadStreamer()

        }



        async created() {

            this.LoadStreamer()

        }



        async Delete() {

            this.$router.push('DeleteStreamer')

        }



        async Edit() {

            this.$router.push('EditStreamer')

        }

    }


</script>