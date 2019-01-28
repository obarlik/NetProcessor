using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BPU
{
    public class Scope
    {
        static long LastScopeNumber = 0;

        public long ScopeNumber;
        public ProcessStep CurrentStep;

        
        public Scope CallerScope;
        public ProcessingStatus Status;
        public string StatusMessage;


        public Scope()
        {
            ScopeNumber = Interlocked.Increment(ref LastScopeNumber);
            Status = ProcessingStatus.Ready;
        }
        

        public async Task AddLog(Context context, string message, params object[] prms)
        {
              await context.AddLog(this, message, prms);
        }


        public async Task SetStatus(Context context, ProcessingStatus status, string message, params object[] prms)
        {
            Status = status;
            StatusMessage = message;
            await AddLog(context, message, prms);
        }


        public async Task Run(Context context)
        {
            if (Status == ProcessingStatus.Running)
            {
                if (CurrentStep is StartStep)
                    await AddLog(context, "Scope {0} started.", ScopeNumber);

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
                await AddLog(context, "Step {0} completed.", ScopeNumber);
            }
        }
    }
}
