using System;
using System.Threading.Tasks;
using ClipHost.ServiceModel.ListStreamerModels;
using ClipHost.ServiceModel.Types;
using ServiceStack;
using ServiceStack.OrmLite;

namespace ClipHost.ServiceModel.ListStreamerService;

public class DeleteStreamer : Service
{
    public async Task<DeleteStreamerResponse> Delete(DeleteStreamerRequest request)
    {


        var data = await Db.SingleAsync(Db.From<Streamer>().Where(a => a.Id == request.Id));

        if (data == null)
        {
            throw new ArgumentException("No Such record");
        }
        Db.Delete(data);
        return new DeleteStreamerResponse()
        {
            DeletedStreamer = data,
            Success = true,

        };

    }
}