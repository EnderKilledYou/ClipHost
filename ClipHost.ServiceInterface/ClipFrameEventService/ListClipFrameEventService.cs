using ClipHost.ServiceModel.ListClipFrameEventModels;
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
using ClipHost.ServiceModel.Types;

namespace ClipHost.ServiceModel.ListClipFrameEventService
{


    public class ListClipFrameEvent : Service
    {



        public async Task<ListClipFrameEventResponse> Get(ListClipFrameEventRequest request)
        {

            try
            {

                var sqlStatement = Db.From<ClipFrameEvent>().Where(a => a.Id > request.After);



                var data = await Db.SelectAsync(sqlStatement);

                return
                new ListClipFrameEventResponse()
                {
                    ClipFrameEvents = data
                ,
                    Success = true
                ,
                    Message = ""
                }
                ;
            }
            catch (Exception e)
            {
                return
new ListClipFrameEventResponse()
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
