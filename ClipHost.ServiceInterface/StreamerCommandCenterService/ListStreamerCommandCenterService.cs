using ClipHost.ServiceModel.ListStreamerCommandCenterModels;
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

namespace ClipHost.ServiceModel.ListStreamerCommandCenterService
{


    public class ListStreamerCommandCenter : Service
    {



        public async Task<ListStreamerCommandCenterResponse> Get(ListStreamerCommandCenterRequest request)
        {

            try
            {

                var sqlStatement = Db.From<StreamerCommandCenter>()
                    .Join<StreamerCommandCenter, Streamer>((streamerCommandCenterTable, streamerTable) =>
                                        streamerCommandCenterTable.StreamerId == streamerTable.Id
                ).Join<StreamerCommandCenter, CommandCenter>((streamerCommandCenterTable, commandCenter) => streamerCommandCenterTable.CommandCenterId == commandCenter.Id)
                .Where<StreamerCommandCenter>(a => a.Id > request.After);

                



                var data = await Db.SelectMultiAsync<StreamerCommandCenter, Streamer, CommandCenter>(sqlStatement);

                return new ListStreamerCommandCenterResponse()
                {
                    StreamerCommandCenters = data,
                    Success = true,
                    Message = ""
                };
            }
            catch (Exception e)
            {
                return new ListStreamerCommandCenterResponse()
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
