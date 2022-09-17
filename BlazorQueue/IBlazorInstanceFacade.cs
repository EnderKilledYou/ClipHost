
 
using System.Threading.Tasks;

namespace BlazorQueue
{

  
    public interface IBlazorInstanceFacade
    {
        Task LocalPublishAllAsync(MetaPacket requestDtos);
        Task LocalPublishAsync(MetaPacket requestDto);
        Task LocalSendAllAsync(MetaPacket requestDtos);
        Task LocalSendAsync(MetaPacket requestDto);

    }
}