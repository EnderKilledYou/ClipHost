using System;
using System.Threading.Tasks;
using ClipHost.ServiceModel.CreateStreamerCommandCenterModels;
using ClipHost.ServiceModel.Types;
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

        request.StreamerCommandCenter.CommandCenterId = commandCenter.Id;
        var exists = Db.Single<StreamerCommandCenter>(a => a.CommandCenterId == commandCenter.Id && a.StreamerId == request.StreamerCommandCenter.StreamerId);
        if (exists == null)
        {
            var id = Db.Insert(request.StreamerCommandCenter, true);
            return
                new CreateStreamerCommandCenterResponse { Id = id };
        }
        throw new ArgumentException("That fucking thing already exists, asshole");


    }
}