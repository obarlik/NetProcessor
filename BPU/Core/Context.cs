using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BPU
{
    public class Context : VariableContainer
    {
        public Guid ContextId;
        public ProcessingStatus Status;
        public string StatusMessage;

        public Dictionary<string, object> Variables { get; private set; }
        public List<Scope> Scopes { get; private set; }

        public Host Host;
        

        public Context()
        {
        }


        public static T Spawn<T>() where T : Context, new()
        {
            return new T()
            {
                ContextId = Guid.NewGuid(),
                Status = ProcessingStatus.Ready,
                StatusMessage = "Ready.",
                Variables = new Dictionary<string, object>(),
                Scopes = new List<Scope>()
            };
        }
        

        public void SetStatus(ProcessingStatus status, string message, params object[] prms)
        {
            Status = status;
            StatusMessage = prms.Any() ? string.Format(message, prms) : message;
            DoUpdate();
            DoLog(null, StatusMessage);
        }


        public void DoLog(Guid? scopeId, string message)
        {
            Host.SaveLog(ContextId, scopeId, message);
        }


        public void DoUpdate()
        {
            Host.SaveContext(this);
        }


        public void Execute()
        {
            SetStatus(ProcessingStatus.Running,
                      "Running");
            
            var runnableScopes =
                Scopes.Where(s => s.Status == ProcessingStatus.Running 
                               || s.Status == ProcessingStatus.AwaitingToRun)
                .ToArray();

            while (Status == ProcessingStatus.Running)
            {
                var runnableScopes = 
                    Scopes.Where(s => s.Status == ProcessingStatus.Running)
                    .ToArray();

                if (!runnableScopes.Any())
                runnableScopes
                .AsParallel()
                .ForAll();

                foreach (var s in Scopes.Where(s => s.Status == ProcessingStatus.Running))
                {
                    i++;
                    Task.Run(() => s.Run());
                }
            }

            SetStatus(ProcessingStatus.Finished,
                      "Finished");
        }


        public void AddScope(Scope scope)
        {
            Scopes.Add(scope);
            scope.DoUpdate();
        }
    }
}
