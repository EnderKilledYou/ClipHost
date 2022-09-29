<template>
    <section>
        <div>
            <h4>Create CommandCenterMask</h4>
            <b-alert :show="true" v-if="DataModel.Message.length >0">{{  DataModel.Message }}</b-alert>
            <b-form-group id="fieldset-Name" description="" label="Name" label-for="input-Name" valid-feedback=""><b-form-input id="fieldset-Name" :disabled="false" v-model="DataModel.Name" type="text" :trim="true"></b-form-input></b-form-group>
            <b-form-group id="fieldset-MaxStreamers" description="" label="MaxStreamers" label-for="input-MaxStreamers" valid-feedback=""><b-form-input id="fieldset-MaxStreamers" :disabled="false" v-model="DataModel.MaxStreamers" type="number" :trim="true"></b-form-input></b-form-group>
            <b-button @click="CreateCommandCenter(DataModel)">Create</b-button>
        </div>
    </section>
</template>
<script lang="ts">
    console.log("");
    import { client } from '@/shared';
    import { CommandCenter, CreateCommandCenterRequest, CreateCommandCenterResponse } from '@/shared/dtos';
    import { Component, Mixins, Vue } from 'vue-property-decorator';

    export class CommandCenterCreateMask extends CommandCenter {

        Message: string = ""

        Success: boolean = true

        Completed: boolean = true

        Error: string = ""

        constructor(init?: Partial<CommandCenterCreateMask>) {

            super()
                ;
            (Object as any).assign(this, init)

        }

    }

    @Component({ components: {} })
    export class CommandCenterApiMixin extends Vue {

        async CreateCommandCenter(DataModel: CommandCenterCreateMask) {

            try {
                const Response: CreateCommandCenterResponse = await client.post(new CreateCommandCenterRequest({ CommandCenter: DataModel }))
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
    export default class CreateCommandCenter extends Mixins(CommandCenterApiMixin) {

        DataModel: CommandCenterCreateMask = new CommandCenterCreateMask(new CommandCenter({}))

    }
</script>