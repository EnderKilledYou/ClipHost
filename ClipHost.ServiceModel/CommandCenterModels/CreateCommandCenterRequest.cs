using ServiceStack;

namespace ClipHost.ServiceModel.CreateCommandCenterModels
{
    public class CreateCommandCenterRequest : IReturn<CreateCommandCenterResponse>
    {
        public CommandCenter CommandCenter { get; set; }
    }
}