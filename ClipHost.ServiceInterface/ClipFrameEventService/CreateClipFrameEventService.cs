using ClipHost.ServiceModel.CreateClipFrameEventModels;
using Microsoft.AspNetCore.Hosting;
using ServiceStack;
using ServiceStack.Auth;
using ServiceStack.Configuration;
using ServiceStack.Logging;
using System.Linq;
using ServiceStack.OrmLite;
using ServiceStack.FluentValidation;
using System;
using System.Threading.Tasks;

namespace ClipHost.ServiceModel.CreateClipFrameEventService
{


    public class CreateClipFrameEvent : ServiceStack.Service
    {



        public async Task<CreateClipFrameEventResponse> Post(CreateClipFrameEventRequest request)
        {

            try
            {
                Db.InsertAll(request.ClipFrameEvent);
                return
                new CreateClipFrameEventResponse() {  }
                ;
            }
            catch (Exception e)
            {
                return
new CreateClipFrameEventResponse()
{
    Success = false
,
    Message = e.Message
}
;
            }
            finally
            {

            }




        }

    }
}
