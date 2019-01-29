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
        public Scope ParentScope;

        public ProcessingStatus Status;
        public string StatusMessage;

        public int ScopeRunCounter;
        public int StepRunCounter;

        public DateTimeOffset CreateTime;
        public DateTimeOffset LastProcessTime;

        public Stack<Guid> VisitedSteps = new Stack<Guid>();
        

        public Scope()
        {
        }


        public static Scope Spawn(ProcessStep startStep, Scope parentScope = null)
        {
            return new Scope()
            {
                ScopeId = Guid.NewGuid(),
                CurrentStep = startStep,
                Context = parentScope == null ? Context.Spawn() : parentScope.Context,
                ParentScope = parentScope,
                Status = ProcessingStatus.Ready,
                StatusMessage = "Ready.",
                ScopeRunCounter = 0,
                StepRunCounter = 0,
                CreateTime = DateTimeOffset.Now,
                LastProcessTime = DateTimeOffset.Now,
            };
        }


        public async Task AddLog(string message, params object[] prms)
        {
            await Context.AddLog(this, message, prms);
        }


        public async Task SetStatus(Context context, ProcessingStatus status, string message, params object[] prms)
        {
            Status = status;
            StatusMessage = message;
            await AddLog(message, prms);
        }


        public async Task Run()
        {
            if (Status == ProcessingStatus.Running)
            {
                ++StepRunCounter;
                ++ScopeRunCounter;

                if (ScopeRunCounter == 1)
                    await AddLog("Step {0} started.", ScopeNumber);

                if (StepRunCounter == 1)
                    await AddLog("Step {0} started.", ScopeNumber);

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
                await AddLog("Step {0} completed, after {1} hits.", ScopeNumber, StepRunCounter);
            }
        }


        public virtual bool UpdateStatus()
        {
            throw new NotImplementedException();
        }
    }
}
