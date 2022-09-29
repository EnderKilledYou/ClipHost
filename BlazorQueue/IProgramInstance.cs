using System.Diagnostics;

namespace BlazorQueue;

public interface IProgramInstance
{
    bool IsConnected();
    void IsConnected(bool value);
    string ConnectionId();
    void ConnectionId(string value);
    Process? Process();
    void RemoteProcessId(int processId);
    int? RemoteProcessId();
    void ProcessDispose();
    bool ProcessExited();
    int ProcessId();
    IProgramInstance Process(Process process);
    ProgramInstanceReport ToReport();
}