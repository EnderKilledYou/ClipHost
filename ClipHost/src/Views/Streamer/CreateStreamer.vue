<template>
    <section>
        <div>
            <h4>Create StreamerMask</h4>
            <b-alert :show="true" v-if="DataModel.Message.length >0">{{  DataModel.Message }}</b-alert>
            <b-form-group id="fieldset-Name" description="" label="Name" label-for="input-Name" valid-feedback="">
                <b-form-input id="fieldset-Name" :disabled="false" v-model="DataModel.Name" type="text" :trim="true"></b-form-input>
            </b-form-group>
            <b-button @click="CreateStreamer(DataModel)">Create</b-button>
        </div>
    </section>
</template>
<script lang="ts">
    console.log("");
    import { Component, Vue } from 'vue-property-decorator'
    import { Mixins } from 'vue-property-decorator'
    import { client } from '@/shared'
    import { CreateStreamerRequest, Streamer, CreateStreamerResponse } from '@/shared/dtos'


    export class StreamerCreateMask extends Streamer {




        Message: string = ""



        Success: boolean = true



        Completed: boolean = true



        Error: string = ""




        constructor(init?: Partial<StreamerCreateMask>) {

            super()
                ;
            (Object as any).assign(this, init)

        }

    }



    @Component({ components: {} })
    export class StreamerApiMixin extends Vue {






        async CreateStreamer(DataModel: StreamerCreateMask) {

            try {
                const Response: CreateStreamerResponse = await client.post(new CreateStreamerRequest({ Streamer: DataModel }))
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
    export default class CreateStreamer extends Mixins(StreamerApiMixin) {




        DataModel: StreamerCreateMask = new StreamerCreateMask(new Streamer({}))



    }


</script>