using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BPU
{
    public class ProcessStep
    {
        public Process ParentProcess;
        public string Name;
        public ProcessStep NextStep;
        public ProcessStep ErrorStep;

        public ProcessingStatus Status { get; private set; }
        public string StatusMessage { get; private set; }


        public T Next<T>(T Step) where T : ProcessStep
        {
            NextStep = Step;
            return Step;
        }


        public async Task SetStatus(Scope scope, ProcessingStatus status, string message, params object[] prms)
        {
            Status = status;
            StatusMessage = message;
            await scope.AddLog(message, prms);
        }


        protected virtual async Task<ProcessStep> Process(Scope scope)
        {
            return await Task.FromResult(NextStep);
        }


        public async Task<ProcessStep> Execute(Scope scope)
        {
            await SetStatus(scope, ProcessingStatus.Running, "Step enter. {0}", Name);

            try
            {
                return await Process(scope);
            }
            catch (Exception ex)
            {
                scope["LastError"] = ex;
                await SetStatus(scope, ProcessingStatus.Halted, "Step error. {0} {1}", Name, ex.Message);

                if (ErrorStep != null)
                    return await Task.FromResult(ErrorStep);

                return await Task.FromResult<ProcessStep>(null);
            }
            finally
            {
                await SetStatus(scope, ProcessingStatus.Finished, "Step leave. {0}", Name);
            }
        }
    }
}
