using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BPU
{
    public class Scope : Dictionary<string, object>
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


        public Scope()
        {
        }


        public Scope(Context context, ProcessStep startStep)
        {
            ScopeId = Guid.NewGuid();
            CurrentStep = startStep;
            Context = context;
            Status = ProcessingStatus.Ready;
            StatusMessage = "Ready.";
            ScopeRunCounter = 0;
            CreateTime = DateTimeOffset.Now;
        }


        public void Retry()
        {
            CanRetry = true;
            Status = ProcessingStatus.Running;
            StatusMessage = "Running.";
        }


        public async Task AddLog(string message)
        {
            await Context.AddLog(this, message);
        }


        public async Task SetStatus(ProcessingStatus status, string message)
        {
            Status = status;
            StatusMessage = message;
            await AddLog(message);
        }


        public async Task Run()
        {
            if (Status == ProcessingStatus.Running)
            {
                if (++ScopeRunCounter == 1)
                    await AddLog($"Scope '{ScopeId}' started.");
                
                var nextStep = await CurrentStep.Execute(this);

                if (nextStep == null)
                    if (CurrentStep is FinishStep)
                        Status = ProcessingStatus.Finished;
                    else
                        Status = ProcessingStatus.Halted;
                else
                    CurrentStep = nextStep;
            }            

            if (Status == ProcessingStatus.Halted
             || Status == ProcessingStatus.Finished
             || Status == ProcessingStatus.Error)
            {
                await AddLog($"Scope {ScopeId} ended, after {ScopeRunCounter} runs.");
            }
        }


        public virtual bool UpdateStatus()
        {
            throw new NotImplementedException();
        }
    }
}
