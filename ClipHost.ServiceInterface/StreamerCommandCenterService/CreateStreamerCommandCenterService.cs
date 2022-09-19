using System;
using System.Threading.Tasks;
using ClipHost.ServiceModel.CreateStreamerCommandCenterModels;
using ServiceStack;
using ServiceStack.OrmLite;

namespace ClipHost.ServiceModel.CreateStreamerCommandCenterService;

public class CreateStreamerCommandCenter : Service
{
    private readonly CommandCenter commandCenter;

    public CreateStreamerCommandCenter(CommandCenter commandCenter)
    {
        this.commandCenter = commandCenter;
    }

    public async Task<CreateStreamerCommandCenterResponse> Post(CreateStreamerCommandCenterRequest request)
    {
        try
        {
            request.StreamerCommandCenter.CommandCenterId = commandCenter.Id;
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