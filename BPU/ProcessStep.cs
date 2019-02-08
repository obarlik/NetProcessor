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

        public List<string> ImportedVariables;
        public List<string> ExportedVariables;


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
            await scope.DoLog($"Step {Name} entered.");

            try
            {
                return await OnExecution(scope);
            }
            catch (Exception ex)
            {
                scope.Variables["$LastError"] = ex;

                await scope.DoLog($"Error while executing {Name} step. Details: {ex.Message}");

                if (ErrorStep == null)
                {
                    await scope.SetStatus(ProcessingStatus.Error, $"Error: {ex.Message}");
                }
                
                return await Task.FromResult(ErrorStep);
            }
            finally
            {
                await scope.DoLog($"Step {Name} left.");
            }
        }
    }
}
