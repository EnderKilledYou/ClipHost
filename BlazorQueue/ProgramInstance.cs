using System.Diagnostics;
using BlazorQueue;

namespace ClipHost
{
    public interface IProgramInstance
    {
        bool IsConnected();
        void IsConnected(bool value);
        string ConnectionId();
        void ConnectionId(string value);
        Process Process();
        int ProcessId();
        IProgramInstance Process(Process process);
        ProgramInstanceReport ToReport();
    }

    public abstract class ProgramInstance : IHaveBlazorConnection, IProgramInstance
    {
        public ProgramInstance()
        {
            IsConnected(false);
            _connectionId = "";
        }

        public IProgramInstance Process(Process process)
        {
            _process = process;
            return this;
        }

        public Process? Process()
        {
            return _process;
        }

        private string _connectionId;
        private bool _isConnected;
        private Process? _process;

        public bool IsConnected()
        {
            return _isConnected;
        }

        public void IsConnected(bool value)
        {
            _isConnected = value;
        }


        public string ConnectionId()
        {
            return _connectionId;
        }

        public void ConnectionId(string value)
        {
            _connectionId = value;
        }

        public abstract ProgramInstanceReport ToReport();

        public int ProcessId()
        {
            return Process()?.Id ?? 0;
        }
    }
}