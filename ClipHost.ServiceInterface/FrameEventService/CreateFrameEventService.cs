using ClipHost.ServiceModel.CreateFrameEventModels;
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

namespace ClipHost.ServiceModel.CreateFrameEventService
{


    public class CreateFrameEvent : ServiceStack.Service
    {



        public async Task<CreateFrameEventResponse> Post(CreateFrameEventRequest request)
        {

            try
            {
                Db.InsertAll(request.FrameEvent);
                return new CreateFrameEventResponse() {   };
            }
            catch (Exception e)
            {
                return new CreateFrameEventResponse()
                {
                    Success = false,
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
