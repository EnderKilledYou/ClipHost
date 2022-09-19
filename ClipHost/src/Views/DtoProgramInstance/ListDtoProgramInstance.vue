<template>
    <section>
        <div>
            <h4>List DtoProgramInstance</h4>
            <b-alert :show="true" v-if="ApiCallMessage && ApiCallMessage.length >0">{{  ApiCallMessage }}</b-alert>
            <b-button-group>
                <b-button @click="Previous">Previous</b-button>
                <b-button @click="ListDtoProgramInstance(DataModel,After)">{{After}}</b-button>
                <b-button @click="Next">Next</b-button>
            </b-button-group>
            <b-form></b-form>

            <b-card no-body>
                <b-tabs card>
                    <b-tab :key="a.DtoId" :title="a.DtoId" v-for=" a  of DataModel" v-model="TabIn" :active="a.DtoId && a.DtoId === DataModel[TabIn].DtoId">
                        <b-progress  :key="b.Id" v-for="b in a.ReportsArray" :value="b.Size" :max=" b.MaxSize" show-progress animated></b-progress>
                    </b-tab>
                </b-tabs>
            </b-card>
        </div>
    </section>
</template>
<script lang="ts">
    console.log("");
    import { Component, Vue } from 'vue-property-decorator'
    import { Mixins } from 'vue-property-decorator'
    import { client } from '@/shared'
    import { ListDtoProgramInstanceRequest, DtoProgramInstance, ListDtoProgramInstanceResponse } from '@/shared/dtos'


    export class DtoProgramInstanceListMask extends DtoProgramInstance {


  

        Message: string = ""



        Success: boolean = true



        Completed: boolean = true



        Error: string = ""




        constructor(originalObject: DtoProgramInstance) {

            super()
            Object.assign(this, originalObject)

        }

    }



    @Component({ components: {} })
    export class DtoProgramInstanceApiMixin extends Vue {




        ApiCallSuccess: boolean = true



        ApiCallMessage: string = ""




        async ListDtoProgramInstance(DataModel: DtoProgramInstanceListMask[], After: number) {

            try {
                const Response: ListDtoProgramInstanceResponse = await client.get(new ListDtoProgramInstanceRequest({ After: After }))
                this.ApiCallSuccess = Response.Success;
                this.ApiCallMessage = Response.Message;
                if (Response.Success) {
                    return Response.DtoProgramInstances.map(a => new DtoProgramInstanceListMask(a))
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
    export class DtoProgramInstanceDataFields extends Mixins(DtoProgramInstanceApiMixin) {




        DataModel: DtoProgramInstanceListMask[] = []



        After: number = 0




        async LoadDtoProgramInstance() {

            this.DataModel = await this.ListDtoProgramInstance(this.DataModel, this.After)

        }

    }



    @Component({ components: {} })
    export default class ListDtoProgramInstance extends Mixins(DtoProgramInstanceDataFields) {

        TabIn: number = 0




        async Previous() {

            this.LoadDtoProgramInstance()

        }



        async Next() {

            this.LoadDtoProgramInstance()

        }



        async created() {

            this.LoadDtoProgramInstance()

        }



        async Delete() {

            this.$router.push('DeleteDtoProgramInstance')

        }



        async Edit() {

            this.$router.push('EditDtoProgramInstance')

        }

    }


</script>