using ClipHost.ServiceModel.ListCommandCenterReportModels;
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

namespace ClipHost.ServiceModel.ListCommandCenterReportService
{


    public class ListCommandCenterReport : Service
    {



        public async Task<ListCommandCenterReportResponse> Get(ListCommandCenterReportRequest request)
        {

            try
            {

                var sqlStatement = Db.From<CommandCenterReport>().Where(a => a.Id > request.After);



                var data = await Db.SelectAsync(sqlStatement);

                return
                new ListCommandCenterReportResponse()
                {
                    CommandCenterReports = data,
                    Success = true,
                    Message = ""
                };
            }
            catch (Exception e)
            {
                return new ListCommandCenterReportResponse()
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
