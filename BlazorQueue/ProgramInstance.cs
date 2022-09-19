using System.Diagnostics;
using BlazorQueue;
using ServiceStack.DataAnnotations;

namespace BlazorQueue
{
    public abstract class ProgramInstance : IHaveBlazorConnection, IProgramInstance, IReportInstance
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

        public void ProcessDispose()
        {
            _process?.Dispose();
        }

        public bool ProcessExited()
        {
            return _process is { HasExited: true };
        }

        private string _connectionId;
        private bool _isConnected;
        private Process? _process;

        public abstract QueueReport[] ReportsArray { get; }

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

        public abstract void UpdateReport(QueueReport report);
    }
}