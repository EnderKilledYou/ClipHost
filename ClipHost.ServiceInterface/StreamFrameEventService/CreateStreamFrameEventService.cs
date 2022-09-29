using ClipHost.ServiceModel.CreateStreamFrameEventModels;
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

namespace ClipHost.ServiceModel.CreateStreamFrameEventService
{


    public class CreateStreamFrameEvent : ServiceStack.Service
    {



        public async Task<CreateStreamFrameEventResponse> Post(CreateStreamFrameEventRequest request)
        {

            try
            {
                Db.InsertAll(request.StreamFrameEvent);
                return
                new CreateStreamFrameEventResponse() { }
                ;
            }
            catch (Exception e)
            {
                return
new CreateStreamFrameEventResponse()
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
