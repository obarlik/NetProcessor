using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BPU
{
    public class Context : VariableDictionary
    {
        public Guid ContextId;
        public List<Scope> Scopes;
        public ProcessingStatus Status;
        public string StatusMessage;

        public event EventHandler OnUpdate;
        public event EventHandler<LogEventArgs> OnLog;


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


        public async Task SetStatus(ProcessingStatus status, string message, params object[] prms)
        {
            Status = status;
            StatusMessage = prms.Any() ? string.Format(message, prms) : message;
            await DoLog(StatusMessage);
        }


        public async Task DoLog(string message)
        {
            var log = new Log(null, ContextId, null, message);

            await Task.Run(() => OnLog?.Invoke(this, new LogEventArgs(log)));
        }


        public async Task DoUpdate()
        {
            await Task.Run(() => OnUpdate?.Invoke(this, EventArgs.Empty));
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



        public void AddScope(Scope scope)
        {
            Scopes.Add(scope);
        }
    }
}
