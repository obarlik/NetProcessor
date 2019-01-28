using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BPU
{
    public class Host : Dictionary<string, object>
    {
        public SubSystem SubSystem;
        public ProcessingStatus Status;
        public string StatusMessage;


        public Host(SubSystem subSystem)
        {
            Status = ProcessingStatus.Ready;
            SubSystem = subSystem;
        }


        public async Task SetStatus(ProcessingStatus status, string message, params object[] prms)
        {
            Status = status;
            StatusMessage = prms.Any() ? string.Format(message, prms) : message;
            await SubSystem.LogProvider.AddLog(null, StatusMessage);
        }


        public IEnumerable<Context> Contexts
        {
            get { return SubSystem.ContextProvider.GetContexts(this); }
        }


        public async Task Run()
        {
            await SetStatus(
                ProcessingStatus.Running, 
                "Host started processing.");

            var contexts = Contexts.ToArray();

            foreach (var c in contexts)
                if (c.Status == ProcessingStatus.Ready
                 || c.Status == ProcessingStatus.Running)
                {
                    await c.Run(this);
                }

            while (Status == ProcessingStatus.Running
                && contexts.Any(c => c.Status == ProcessingStatus.Running))
            {
                Thread.Sleep(1000);
            }

            await SetStatus(
                ProcessingStatus.Finished, 
                "Host finished.");
        }
    }
}
