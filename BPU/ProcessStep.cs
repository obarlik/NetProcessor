using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BPU
{
    public class ProcessStep
    {
        public Process Process;
        public string Name;
        public ProcessStep NextStep;
        public ProcessStep ErrorStep;


        public string UniqueName
        {
            get { return $"{Process.Name}/{Name}"; }
        }


        protected virtual async Task<ProcessStep> OnExecution(Scope scope)
        {
            return await Task.FromResult(NextStep);
        }


        public async Task<ProcessStep> Execute(Scope scope)
        {
            await scope.AddLog($"Step entrance. {Name}");

            try
            {
                return await OnExecution(scope);
            }
            catch (Exception ex)
            {
                scope["$LastError"] = ex;

                await scope.AddLog("Step error. {0} {1}", Name, ex.Message);

                if (ErrorStep == null)
                {
                    await scope.SetStatus(
                        ProcessingStatus.Error,
                        "Error: " + ex.Message);
                }
                
                return await Task.FromResult(ErrorStep);
            }
            finally
            {
                await scope.AddLog("Step exit. {0}", Name);
            }
        }
    }
}
