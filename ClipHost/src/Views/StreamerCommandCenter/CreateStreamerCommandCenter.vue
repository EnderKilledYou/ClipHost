<template>
    <section>
        <div>
            <h4>Assign Streamer</h4>
            <b-alert :show="true" v-if="DataModel.Message.length >0">{{  DataModel.Message }}</b-alert>
            <b-form-group id="fieldset-StreamerId" description="" label="Streamer" label-for="input-StreamerId" valid-feedback="">

                <b-form-select v-model="DataModel.StreamerId" class="mb-3">
                    <b-form-select-option :value="0">Please select an option</b-form-select-option>
                    <b-form-select-option :key="a.Id" :value="a.Id" v-for="a in streamers">{{a.Name}}</b-form-select-option>
                </b-form-select>

            </b-form-group>

            <b-button @click="CreateStreamerCommandCenter(DataModel)">Create</b-button>
        </div>
    </section>
</template>
<script lang="ts">
    console.log("");
    import { Component, Vue } from 'vue-property-decorator'
    import { Mixins } from 'vue-property-decorator'
    import StreamerCommandCenterApiMixin, { StreamerCommandCenterCreateMask } from './StreamerCommandCenter'
    import StreamerApiMixin, { StreamerListMask } from '@/Views/Streamer/ListStreamerMix'
    import { StreamerCommandCenter } from '@/shared/dtos'






    @Component({ components: {} })
    export default class CreateStreamerCommandCenter extends Mixins(StreamerCommandCenterApiMixin, StreamerApiMixin, StreamerCommandCenterApiMixin) {
        async created() {



            this.streamers = await this.ListStreamer([], 0, "");;

        }
        streamers: StreamerListMask[] = []
        selectedStreamer: number = -1;

        DataModel: StreamerCommandCenterCreateMask = new StreamerCommandCenterCreateMask(new StreamerCommandCenter({ Id: 1, StreamerId: 0 }))



    }


</script>