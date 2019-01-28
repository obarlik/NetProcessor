using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BPU
{
    public class Scope : Dictionary<string, object>
    {
        static long LastScopeNumber = 0;

        public long ScopeNumber;
        public Context Context;
        public ProcessStep CurrentStep;
        public Scope CallerScope;
        public Dictionary<string, object> Parameters;
        public object Result;
        public ProcessingStatus Status;
        public string StatusMessage;


        public Scope()
        {
            ScopeNumber = Interlocked.Increment(ref LastScopeNumber);
            Status = ProcessingStatus.Ready;
        }


        public async Task AddLog(string message, params object[] prms)
        {
            await Context.AddLog(this, message, prms);
        }


        public async Task Run(Context context)
        {
            Context = context;

            Status = ProcessingStatus.Running;
            await AddLog("Scope {0} started.", ScopeNumber);

            while (Status == ProcessingStatus.Running 
                && CurrentStep != null)
            {
                try
                {
                    CurrentStep = await CurrentStep.Process(this);
                }
                catch(Exception ex)
                {
                    Status = ProcessingStatus.Halted;
                    StatusMessage = ex.Message;
                    await AddLog(
                        "Scope {0} halted with error message '{1}'.", 
                        ScopeNumber, 
                        StatusMessage);
                }
            }

            Status = ProcessingStatus.Finished;
            await AddLog("Scope {0} completed.", ScopeNumber);
        }
    }
}
