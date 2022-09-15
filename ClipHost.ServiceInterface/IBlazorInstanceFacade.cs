using System.Threading.Tasks;

namespace BlazorQueue.ServiceInterface
{
    public interface IBlazorInstanceFacade
    {
        Task LocalPublishAllAsync(object requestDtos);
        Task LocalPublishAsync(object requestDto);
        Task LocalSendAllAsync(object requestDtos);
        Task LocalSendAsync(object requestDto);
     
    }
}