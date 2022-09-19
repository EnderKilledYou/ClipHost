import { Component, Vue } from 'vue-property-decorator'
import { ListStreamerRequest, Streamer, ListStreamerResponse } from '@/shared/dtos'

import { client } from '@/shared'

@Component({ components: {} })
export default class StreamerApiMixin extends Vue {




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
            debugger;
            if (Response.Success) {
                return Response.Streamers.map(a => new StreamerListMask(a))
            }
            return []
        } catch (e: any) {
            debugger;
            this.ApiCallMessage = e.message;
            console.log(e); return []
        } finally {

        }




    }

}

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