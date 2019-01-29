using System;
using System.Collections.Generic;
using System.Linq;
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


        protected virtual async Task<ProcessStep> Process(Scope scope)
        {
            return await Task.FromResult(NextStep);
        }


        public async Task<ProcessStep> Execute(Scope scope)
        {
            await scope.AddLog("Step entrance. {0}", Name);

            try
            {
                return await Process(scope);
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

                    if (!scope.Context.Scopes.Any(s => s.ScopeId != scope.ScopeId))
                    {
                        scope.Context["$LastError"] = ex;

                        await scope.Context.SetStatus(
                            ProcessingStatus.Error,
                            "Scope " + scope.ScopeId + " threw an unexpected error '" + ex.Message + "'");
                    }

                    if (scope.CallerScope != null)
                    {
                        scope.CallerScope.Status = ProcessingStatus.Error;
                        scope.CallerScope.StatusMessage = "Scope " + scope.CallerScope.ScopeId + " has called scope " + scope.ScopeId + " but it threw an unexpected error '" + ex.Message + "'";
                    }
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
