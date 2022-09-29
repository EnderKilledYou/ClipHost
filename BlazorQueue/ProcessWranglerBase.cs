using Microsoft.Extensions.Hosting;
using ServiceStack.Configuration;
using ServiceStack.Data;
using System.Diagnostics;
using BlazorQueue;
using ServiceStack;

namespace ClipHost
{
    public class ProcessWranglerBase<T> : BackgroundService, IAcceptConnections where T : IProgramInstance, new()
    {
        protected readonly List<T> dtoProgramInstances;
        protected readonly List<T> remoteDtoProgramInstances;
        protected readonly string HostUrl;
        protected int? _maxInstances;
        protected readonly string ClipHuntaProcessPath;
        protected readonly IAppSettings settings;

        protected ProcessWranglerBase(IAppSettings appSettings, int maxProcesses)
        {
            dtoProgramInstances = new List<T>();
            remoteDtoProgramInstances = new List<T>();
            HostUrl = appSettings.Get<string>("ClipHuntaUrlSetting");
            _maxInstances = maxProcesses;
            ClipHuntaProcessPath = appSettings.Get<string>("ClipHuntaProcessPath");
            settings = appSettings;
        }

        /// <summary>
        ///  
        /// </summary>
        /// <returns>The amount of available slots</returns>
        public virtual int Count()
        {
            return dtoProgramInstances.Count(b => !string.IsNullOrEmpty(b.ConnectionId()));
        }
        /// <summary>
        ///  
        /// </summary>
        /// <returns>The amount of available slots</returns>
        public virtual int CountRemote()
        {
            return remoteDtoProgramInstances.Count(b => !string.IsNullOrEmpty(b.ConnectionId()));
        }
        /// <summary>
        /// Set max instances
        /// </summary>
        /// <param name="max"></param>
        /// <exception cref="ArgumentException"></exception>
        public void MaxInstances(int max)
        {
            if (max < 1)
            {
                throw new ArgumentException("Max must be greater than 1");
            }

            _maxInstances = max;
        }

        /// <summary>
        /// Set the connection id for the process id
        /// </summary>
        /// <param name="connectionId"></param>
        /// <param name="processId"></param>
        /// <exception cref="ArgumentException">If no such process exists, i.e. it started, stopped, and this message lagged</exception>
        public void SetConnectionId(string connectionId, int processId)
        {
            var instance = dtoProgramInstances.FirstOrDefault(a => a.Process().Id == processId);
            if (instance == null)
            {
                throw new ArgumentException("No such process");
            }

            instance.ConnectionId(connectionId);
            instance.IsConnected(true);
        }

        public void AddRemoteConnectionId(string connectionId, int processId)
        {
            var instance = remoteDtoProgramInstances.FirstOrDefault(a => a.RemoteProcessId() == processId);
            if (instance != null)
            {
                throw new ArgumentException("process already exists");
            }
            instance = new T();

            instance.RemoteProcessId(processId);
            instance.ConnectionId(connectionId);
            instance.IsConnected(true);
            remoteDtoProgramInstances.Add(instance);
        }


        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await Task.Delay(1, stoppingToken);
        }

        /// <summary>
        /// Checks the processes for stopped ones and clears them out
        /// </summary>
        protected virtual void CleanDeadProcesses()
        {
            for (var i = dtoProgramInstances.Count - 1; i >= 0; i--)
            {
                var dtoProgramInstance = dtoProgramInstances[i];
                if (!dtoProgramInstance.ProcessExited()) continue;
                dtoProgramInstances.RemoveAt(i);
                dtoProgramInstance.ProcessDispose();
            }
        }

        /// <summary>
        /// Converts the list of processes to a list of status reports.
        /// </summary>
        /// <param name="items"></param>
        public virtual void Report(List<ProgramInstanceReport> items)
        {
            items.AddRange(dtoProgramInstances.Select(item => item.ToReport()));
        }

        protected virtual T? StartProgram()
        {
            var p = ProcessHelper.StartProcess(ClipHuntaProcessPath, HostUrl);
            if (p == null) return default;
            if (!p.Start()) return default;
            var programInstance = new T();
            programInstance.Process(p);
            dtoProgramInstances.Add(programInstance);
            return programInstance;
            //todo: when it doesn't start or otherwise. could burdened surver
            //todo: when it doesn't start or otherwise. could burdened surver
        }

        protected virtual void StartPrograms()
        {
            if (_maxInstances == null)
            {
                throw new ArgumentException(
                    "No max instances, set ClipHuntaMaxInstances in app settings for initial value or call MaxInstances(i)");
            }

            var count = _maxInstances - dtoProgramInstances.Count;
            for (var i = 0; i < count; i++)
            {
                StartProgram();
            }
        }
    }
}