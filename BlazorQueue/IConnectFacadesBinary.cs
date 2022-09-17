namespace BlazorQueue
{
    public interface IConnectFacadesBinary : IConnectFacades
    {
        public IConnectFacadesBinary GetParentFacade();

        public IConnectFacadesBinary GetLeftFacade();

        public void SetLeftFacade(IConnectFacadesBinary left);

        public void SetRightFacade(IConnectFacadesBinary right);


        public IConnectFacadesBinary GetRightFacade();
    }
}
