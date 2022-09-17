using System;
using System.Threading.Tasks;
using ClipHost.ServiceModel.CreateStreamerModels;
using ServiceStack;
using ServiceStack.OrmLite;

namespace ClipHost.ServiceModel.CreateStreamerService;

public class CreateStreamer : Service
{
    public async Task<CreateStreamerResponse> Post(CreateStreamerRequest request)
    {
        try
        {
            var id = Db.Insert(request.Streamer, true);
            return
                new CreateStreamerResponse { Id = id };
        }
        catch (Exception e)
        {
            return new CreateStreamerResponse
            {
                Success = false,
                Message = e.Message
            };
        }
    }
}