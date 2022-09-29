using ClipHost.ServiceModel.ListStreamFrameEventModels;
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

namespace ClipHost.ServiceModel.ListStreamFrameEventService
{


    public class ListStreamFrameEvent : Service
    {



        public async Task<ListStreamFrameEventResponse> Get(ListStreamFrameEventRequest request)
        {

            try
            {

                var sqlStatement = Db.From<StreamFrameEvent>().Where(a => a.Id > request.After);



                var data = await Db.SelectAsync(sqlStatement);

                return
                new ListStreamFrameEventResponse()
                {
                    StreamFrameEvents = data
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
new ListStreamFrameEventResponse()
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
