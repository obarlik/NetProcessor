using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BPU
{
    public class Scope : IVariableContainer
    {
        public Guid ScopeId;

        public Context Context;
        public ProcessStep CurrentStep;

        ILogProvider LogProvider;

        public Dictionary<string, object> Variables { get; set; }
        public Stack<Guid> ReturnStepIds { get; set; }
        public Stack<Guid> VisitedStepIds { get; set; }
    
        public ProcessingStatus Status;
        public string StatusMessage;

        public int ScopeRunCounter;

        public DateTimeOffset CreateTime;
        public DateTimeOffset? LastProcessTime;

        public int RetryCount;
        public bool CanRetry;
        public Exception LastError;


        public Scope(ILogProvider logProvider)
        {
            LogProvider = logProvider;
        }


        public Scope(ILogProvider logProvider, Context context, ProcessStep currentStep)
            : this(logProvider)
        {
            Variables = new Dictionary<string, object>();
            ReturnStepIds = new Stack<Guid>();
            VisitedStepIds = new Stack<Guid>();

            ScopeId = Guid.NewGuid();
            Context = context;
            ScopeRunCounter = 0;
            CreateTime = DateTimeOffset.Now;
            Status = ProcessingStatus.Ready;
            StatusMessage = "Ready.";
            CurrentStep = currentStep;
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


        public void Execute()
        {
            SetStatus(ProcessingStatus.Running, "Running.");
            
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


        public virtual void Save()
        {
        }
    }
}
