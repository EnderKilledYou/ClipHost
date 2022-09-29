using ClipHost.ServiceModel.ListProcessReportModels;
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

namespace ClipHost.ServiceModel.ListProcessReportService
{


    public class ListProcessReport : Service
    {

        CommandCenter commandCenter;

        public ListProcessReport(CommandCenter streamerCommandCenter)
        {
            this.commandCenter = streamerCommandCenter;
        }

        public async Task<ListProcessReportResponse> Get(ListProcessReportRequest request)
        {

            try
            {

                var sqlStatement = Db.From<ProcessReport>().Where(a => a.Id > request.After);




                var processReports = Db.From<ProcessReport>()
                    .LeftJoin<ProcessReport, StreamerCommandCenter>((b, a) => a.Id == b.StreamerCommandCenterId)
                    .LeftJoin<StreamerCommandCenter, Streamer>((streamerCommandCenterTable, streamerTable)
                    => streamerCommandCenterTable.StreamerId == streamerTable.Id);
                // .Where<StreamerCommandCenter>(a => a.CommandCenterId == commandCenter.Id);

                if (request.IsRunning != null)
                    processReports = processReports.Where<ProcessReport>(a => a.IsRunning == request.IsRunning);
                if (request.ProcessId > 0)
                    processReports = processReports.Where<ProcessReport>(a => a.ProcessId == request.ProcessId);

                var data = await Db.SelectMultiAsync<ProcessReport, StreamerCommandCenter, Streamer>(processReports);

                return new ListProcessReportResponse()
                {
                    //    ProcessReports = data
                    //   ,
                    Success = true
                ,
                    Message = ""
                }
                ;
            }
            catch (Exception e)
            {
                return new ListProcessReportResponse()
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
