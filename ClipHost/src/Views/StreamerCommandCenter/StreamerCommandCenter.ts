import { Component, Vue } from 'vue-property-decorator'
import { CreateStreamerCommandCenterRequest, StreamerCommandCenter, CreateStreamerCommandCenterResponse } from '@/shared/dtos'
import { client } from '@/shared'
@Component({ components: {} })
export default class StreamerCommandCenterApiMixin extends Vue {






    async CreateStreamerCommandCenter(DataModel: StreamerCommandCenterCreateMask) {

        try {
            const Response: CreateStreamerCommandCenterResponse = await client.post(new CreateStreamerCommandCenterRequest({ StreamerCommandCenter: DataModel }))
            DataModel.Success = Response.Success;
            if (Response.Id > 0) { DataModel.Message = 'Created' } else { DataModel.Message = Response.Message; }
        } catch (e: any) {
            DataModel.Message = e.message;
            console.log(e)
        } finally {

        }




    }

}

export class StreamerCommandCenterCreateMask extends StreamerCommandCenter {




    Message: string = ""



    Success: boolean = true



    Completed: boolean = true



    Error: string = ""




    constructor(init?: Partial<StreamerCommandCenterCreateMask>) {

        super()
            ;
        (Object as any).assign(this, init)

    }

}


