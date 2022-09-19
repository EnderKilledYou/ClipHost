namespace ClipHost
{
    public record AssignDtoResult
    {
        public enum EReason
        {
            NoProcess,
            NoClient,
            Started
        }
        private readonly bool started;
        private readonly string reason;
        private readonly EReason eReason;
        private readonly ProgramInstance? instance;

        public AssignDtoResult(bool started, string reason, EReason eReason, ProgramInstance? instance = null)
        {
            this.started = started;
            this.reason = reason;
            this.eReason = eReason;
            this.instance = instance;
        }

        public bool Started => started;

        public string ReasonString => reason;

        public EReason Reason => eReason;

        public ProgramInstance? Instance => instance;
    }


}