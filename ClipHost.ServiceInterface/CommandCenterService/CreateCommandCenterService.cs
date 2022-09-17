using System;
using System.Threading.Tasks;
using ClipHost.ServiceModel.CreateCommandCenterModels;
using ServiceStack;
using ServiceStack.OrmLite;

namespace ClipHost.ServiceModel.CreateCommandCenterService;

public class CreateCommandCenter : Service
{
    public async Task<CreateCommandCenterResponse> Post(CreateCommandCenterRequest request)
    {
        try
        {
            var id = Db.Insert(request.CommandCenter, true);
            return new CreateCommandCenterResponse { Id = id };
        }
        catch (Exception e)
        {
            return new CreateCommandCenterResponse
            {
                Success = false,
                Message = e.Message
            };
        }
    }
}