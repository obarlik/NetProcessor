using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BPU
{
    public class Scope : VariableDictionary
    {
        public Guid ScopeId;
        public ProcessStep CurrentStep;

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
        }


        public void ImportVariable(string variableName)
        {
            SetVariable(variableName, Context.GetVariable(variableName));
        }
        

        public void ExportVariable(string variableName)
        {
            Context.SetVariable(variableName, GetVariable(variableName));
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

            return scope;
        }


        public async Task DoUpdate()
        {
            await Task.Run(() => OnUpdate?.Invoke(this, EventArgs.Empty));
        }


        public async Task DoLog(string message)
        {
            await Task.Run(() => OnLog?.Invoke(this, new LogEventArgs(
                new Log(null, Context?.ContextId, ScopeId, message))));
        }


        public void Retry()
        {
            CanRetry = true;
            Status = ProcessingStatus.Running;
            StatusMessage = "Running.";
        }
        

        public async Task SetStatus(ProcessingStatus status, string message)
        {
            Status = status;
            StatusMessage = message;
            await DoLog(message);
        }


        public async Task Run()
        {
            if (Status == ProcessingStatus.Running)
            {
                if (++ScopeRunCounter == 1)
                    await DoLog($"Scope {ScopeId} started.");
                
                var nextStep = await CurrentStep.Execute(this);

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
                await DoLog($"Scope {ScopeId} ended, after {ScopeRunCounter} runs.");
            }
        }
    }
}
