using ClipHost.ServiceModel.CreateCommandCenterModels;
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

namespace ClipHost.ServiceModel.CreateCommandCenterService
{


    public class CreateCommandCenter : ServiceStack.Service
    {



        public async Task<CreateCommandCenterResponse> Post(CreateCommandCenterRequest request)
        {

            try
            {
                var id = Db.Insert(request.CommandCenter, true);
                return new CreateCommandCenterResponse() { Id = id }               ;
            }
            catch (Exception e)
            {
                return     new CreateCommandCenterResponse() {
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
