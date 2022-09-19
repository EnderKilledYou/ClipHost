namespace ClipHost
{
    public interface IHaveBlazorConnection
    {
        public string ConnectionId();
        public void ConnectionId(string value);
        public bool IsConnected();
        public void IsConnected(bool value);
       
    }
}