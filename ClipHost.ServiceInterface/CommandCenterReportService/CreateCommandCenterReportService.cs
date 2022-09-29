using ClipHost.ServiceModel.CreateCommandCenterReportModels;
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

namespace ClipHost.ServiceModel.CreateCommandCenterReportService
{


    public class CreateCommandCenterReport : ServiceStack.Service
    {



        public async Task<CreateCommandCenterReportResponse> Post(CreateCommandCenterReportRequest request)
        {

            try
            {
                if (request.CommandCenterReport.Id > 0)
                {
                    var exists = Db.Single<CommandCenterReport>(a => a.Id == request.CommandCenterReport.Id);
                    if (exists != null)
                    {
                        Db.Update(request.CommandCenterReport);
                        return new CreateCommandCenterReportResponse() { Id = exists.Id };
                    }
                }

                var id = Db.Insert(request.CommandCenterReport, true);
                return new CreateCommandCenterReportResponse() { Id = id };
            }
            catch (Exception e)
            {
                return new CreateCommandCenterReportResponse()
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
