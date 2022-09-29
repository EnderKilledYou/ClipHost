using ServiceStack;

namespace ClipHost.ServiceModel.ListStreamerModels
{
    public class DeleteStreamerRequest : IReturn<DeleteStreamerResponse>
    {


        public int Id { get; set; }




    }
}
