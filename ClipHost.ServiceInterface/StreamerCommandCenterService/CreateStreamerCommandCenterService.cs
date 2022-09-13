using ClipHost.ServiceModel.CreateStreamerCommandCenterModels;
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

namespace ClipHost.ServiceModel.CreateStreamerCommandCenterService
{


    public class CreateStreamerCommandCenter : ServiceStack.Service
    {



        public async Task<CreateStreamerCommandCenterResponse> Post(CreateStreamerCommandCenterRequest request)
        {

            try
            {
                var id = Db.Insert(request.StreamerCommandCenter, true);
                return
                new CreateStreamerCommandCenterResponse() { Id = id };
            }
            catch (Exception e)
            {
                return new CreateStreamerCommandCenterResponse()
                {
                    Success = false,

                    Message = e.Message
                };
            }
            finally
            {

            }




        }

    }
}
