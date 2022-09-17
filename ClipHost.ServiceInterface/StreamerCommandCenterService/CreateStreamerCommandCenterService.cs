using System;
using System.Threading.Tasks;
using ClipHost.ServiceModel.CreateStreamerCommandCenterModels;
using ServiceStack;
using ServiceStack.OrmLite;

namespace ClipHost.ServiceModel.CreateStreamerCommandCenterService;

public class CreateStreamerCommandCenter : Service
{
    public async Task<CreateStreamerCommandCenterResponse> Post(CreateStreamerCommandCenterRequest request)
    {
        try
        {
            var id = Db.Insert(request.StreamerCommandCenter, true);
            return
                new CreateStreamerCommandCenterResponse { Id = id };
        }
        catch (Exception e)
        {
            return new CreateStreamerCommandCenterResponse
            {
                Success = false,

                Message = e.Message
            };
        }
    }
}