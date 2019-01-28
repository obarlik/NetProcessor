using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BPU
{
    public class Context : Dictionary<string, object>
    {
        public List<Scope> Scopes;
        public List<Log> Logs;
        public Host Host;
        public ProcessingStatus Status;
        public string StatusMessage;


        public Context()
        {
            Status = ProcessingStatus.Ready;
            StatusMessage = "Ready";
        }


        public async Task AddLog(Scope scope, string message, params object[] prms)
        {
            await Host.SubSystem.LogProvider.AddLog(scope, message, prms);
        }


        public async Task Run(Host host)
        {
            Host = host;

            Status = ProcessingStatus.Running;
            StatusMessage = "Running";
            
            foreach (var s in Scopes)
                if (s.Status == ProcessingStatus.Running)
                    await s.Run(this);

            while (Status == ProcessingStatus.Running)
            {
            }
        }
    }
}
