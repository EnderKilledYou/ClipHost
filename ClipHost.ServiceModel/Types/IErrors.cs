namespace ClipHost.ServiceModel.Types
{
    public interface IErrors
    {
        public string Message { get; set; }
        public string StackTrace { get; set; }
        public string Context { get; set; }
        public string Location { get; set; }
    }
}
