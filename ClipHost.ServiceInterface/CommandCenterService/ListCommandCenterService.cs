using ClipHost.ServiceModel.ListCommandCenterModels;
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

namespace ClipHost.ServiceModel.ListCommandCenterService
{


    public class ListCommandCenter : Service
    {



        public async Task<ListCommandCenterResponse> Get(ListCommandCenterRequest request)
        {

            try
            {

                var sqlStatement = Db.From<CommandCenter>().Where(a => a.Id > request.After);



                var data = await Db.SelectAsync(sqlStatement);

                return
                new ListCommandCenterResponse()
                {
                    CommandCenters = data,
                    Success = true,
                    Message = ""
                };
            }
            catch (Exception e)
            {
                return new ListCommandCenterResponse()
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
