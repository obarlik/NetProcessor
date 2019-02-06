using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BPU
{
    public class Context : Dictionary<string, object>
    {
        public Guid ContextId;
        public List<Scope> Scopes;
        public ProcessingStatus Status;
        public string StatusMessage;


        public Context()
        {
        }

        
        public static Context Spawn()
        {
            return new Context()
            {
                ContextId = Guid.NewGuid(),
                Scopes = new List<Scope>(),
                Status = ProcessingStatus.Ready,
                StatusMessage = "Ready."
            };
        }



        public async Task AddLog(Scope scope, string message)
        {
            await Host.Instance.AddLog(ContextId, scope.ScopeId, message);
        }


        public async Task SetStatus(ProcessingStatus status, string message, params object[] prms)
        {
            Status = status;
            StatusMessage = prms.Any() ? string.Format(message, prms) : message;
            await AddLog(null, StatusMessage);
        }


        public async Task Execute()
        { 
            Status = ProcessingStatus.Running;
            StatusMessage = "Running";
            var i = 1;

            while (Status == ProcessingStatus.Running
                && i == 1)
            {
                i = 0;

                foreach (var s in Scopes.Where(s => s.Status == ProcessingStatus.Running))
                {
                    i++;
                    await s.Run();
                }
            }

            Status = ProcessingStatus.Finished;
            StatusMessage = "Finished";
        }
    }
}
