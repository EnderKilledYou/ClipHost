using System;
using System.Threading.Tasks;
using ClipHost.ServiceModel.ListStreamerModels;
using ClipHost.ServiceModel.Types;
using ServiceStack;
using ServiceStack.OrmLite;

namespace ClipHost.ServiceModel.ListStreamerService;

public class ListStreamer : Service
{
    public async Task<ListStreamerResponse> Get(ListStreamerRequest request)
    {
        try
        {
            var sqlStatement = Db.From<Streamer>().Where(a => a.Id > request.After);


            sqlStatement = sqlStatement.Where(a => a.Name.Contains(request.Name));

            var data = await Db.SelectAsync(sqlStatement);

            return
                new ListStreamerResponse
                { Streamers = data, Success = true, Message = "" }
                ;
        }
        catch (Exception e)
        {
            return
                new ListStreamerResponse
                { Success = false, Message = e.Message }
                ;
        }
    }
}
