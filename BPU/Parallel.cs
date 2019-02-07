using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BPU
{
    public class Parallel : ProcessStep
    {
        public ProcessStep ParallelStep;

        protected override async Task<ProcessStep> OnExecution(Scope scope)
        {
            var newScope = scope.Context.SpawnScope(ParallelStep);

            await newScope.SetStatus(ProcessingStatus.Running, "Running.");
            
            return await base.OnExecution(scope);
        }
    }
}
