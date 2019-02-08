using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BPU
{
    public class Scope : VariableContainer
    {
        public Guid ScopeId;
        public ProcessStep CurrentStep;

        public Dictionary<string, object> Variables { get; private set; }

        public Context Context;
        public Stack<ProcessStep> ReturnSteps = new Stack<ProcessStep>();

        public ProcessingStatus Status;
        public string StatusMessage;

        public int ScopeRunCounter;

        public DateTimeOffset CreateTime;
        public DateTimeOffset? LastProcessTime;

        public Stack<Guid> VisitedSteps = new Stack<Guid>();

        public int RetryCount;
        public bool CanRetry;

        public event EventHandler OnUpdate;

        public event EventHandler<LogEventArgs> OnLog;


        public Scope()
        {
            Variables = new Dictionary<string, object>();
        }


        public void ImportVariable(string variableName)
        {
            this.SetVariable(
                variableName, 
                Context.GetVariable(variableName));
        }
        

        public void ExportVariable(string variableName)
        {
            Context.SetVariable(
                variableName, 
                this.GetVariable(variableName));
        }
        

        public static Scope Spawn(Context context, ProcessStep startStep)
        {
            var scope = new Scope()
            {
                ScopeId = Guid.NewGuid(),
                Context = context,
                ScopeRunCounter = 0,
                CreateTime = DateTimeOffset.Now,
                Status = ProcessingStatus.Ready,
                StatusMessage = "Ready.",
            };

            context.AddScope(scope);

            context.DoUpdate();

            return scope;
        }


        public void DoUpdate()
        {
            OnUpdate?.Invoke(this, EventArgs.Empty);
        }


        public void DoLog(string message)
        {
            OnLog?.Invoke(this, new LogEventArgs(null, Context?.ContextId, ScopeId, message));
        }


        public void Retry()
        {
            CanRetry = true;
            Status = ProcessingStatus.Running;
            StatusMessage = "Running.";
        }
        

        public void SetStatus(ProcessingStatus status, string message)
        {
            Status = status;
            StatusMessage = message;
            DoLog(message);
        }


        public void Run()
        {
            if (Status == ProcessingStatus.Running)
            {
                if (++ScopeRunCounter == 1)
                    DoLog($"Scope {ScopeId} started.");
                
                var nextStep = CurrentStep.Execute(this);

                if (nextStep == null)
                {
                    if (CurrentStep is FinishStep)
                        Status = ProcessingStatus.Finished;
                    else
                        Status = ProcessingStatus.Halted;
                }

                CurrentStep = nextStep;
            }            

            if (Status == ProcessingStatus.Halted
             || Status == ProcessingStatus.Finished
             || Status == ProcessingStatus.Error)
            {
                DoLog($"Scope {ScopeId} ended, after {ScopeRunCounter} runs.");
            }
        }
    }
}
