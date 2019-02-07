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
            return await Task.Run(async () =>
            {
                Scope.Spawn(scope.Context, ParallelStep);
                return await base.OnExecution(scope);
            });
        }
    }
}
