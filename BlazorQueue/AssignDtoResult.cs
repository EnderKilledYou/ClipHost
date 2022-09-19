namespace BlazorQueue
{
    public record AssignDtoResult(bool Started, string ReasonString, AssignDtoResult.EReason Reason, ProgramInstance? Instance = null)
    {
        public enum EReason
        {
            NoProcess,
            NoClient,
            Started
        }
    }


}