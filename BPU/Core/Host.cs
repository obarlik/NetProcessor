using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BPU
{
    public abstract class Host : SubSystem
    {
        public string Name;
        public ProcessingStatus Status;
        public string StatusMessage;


        public Host()
        {
            Status = ProcessingStatus.Ready;
        }


        public abstract void SaveLog(Log log);

        public abstract IEnumerable<Process> GetProcesses();
        public abstract IEnumerable<Context> GetContexts();
        public abstract void SaveContext(Context context);


        public void SetStatus(ProcessingStatus status, string message, params object[] prms)
        {
            Status = status;
            StatusMessage = prms.Length > 0 ? string.Format(message, prms) : message;

            SaveLog(new Log(Name, null, null, StatusMessage));
        }


        public void SaveLog(Guid? contextId, Guid? scopeId, string message)
        {
            SaveLog(new Log(Name, contextId, scopeId, message));
        }

        
        public void Run()
        {
            SetStatus(ProcessingStatus.Running, 
                      "Host started processing.");

            while (Status == ProcessingStatus.Running)
            {
                foreach (var c in GetContexts()
                                  .Where(c => c.Status == ProcessingStatus.Running)
                                  .ToArray())
                {
                    Task.Run(() => c.Execute());
                }

                Thread.Sleep(1000);
            }

            SetStatus(ProcessingStatus.Finished, 
                      "Host finished.");
        }


        public virtual Process NewProcess()
        {
            return new Process();
        }


        public abstract void SaveProcess(Process process);


        public virtual Context NewContext()
        {
            return new Context();
        }


        public Scope NewScope(Context context)
        {
            var scope = new Scope();
            context.AddScope(scope);
            return scope;
        }


        public virtual void SaveScope(Scope scope)
        {
            SaveContext(scope.Context);
        }
    }
}
