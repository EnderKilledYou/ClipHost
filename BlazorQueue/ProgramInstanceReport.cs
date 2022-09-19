using ServiceStack.DataAnnotations;

namespace BlazorQueue;

public  abstract class ProgramInstanceReport
{
    public int ProcessId { get; }

    public ProgramInstanceReport(string connectionId, int id)
    {
        ProcessId = id;
 
        ConnectionId = connectionId;
    }


 
    public string ConnectionId { get; }
}
