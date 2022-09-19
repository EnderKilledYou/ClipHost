namespace BlazorQueue
{
    public interface IAcceptConnections
    {
 
        void SetConnectionId(string connectionId, int processId);
    }
}