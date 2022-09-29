using ClipHost.ServiceModel.ListFrameEventModels;
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

namespace ClipHost.ServiceModel.ListFrameEventService
{


    public class ListFrameEvent : Service
    {



        public async Task<ListFrameEventResponse> Get(ListFrameEventRequest request)
        {

            try
            {

                var sqlStatement = Db.From<StreamFrameEvent>().Where(a => a.Id > request.After);



                var data = await Db.SelectAsync(sqlStatement);

                return
                new ListFrameEventResponse()
                {
          //          StreamFrameEvent = data
            //    ,
                    Success = true
                ,
                    Message = ""
                }
                ;
            }
            catch (Exception e)
            {
                return
new ListFrameEventResponse()
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
