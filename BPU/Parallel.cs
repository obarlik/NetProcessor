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
            var newScope = new Scope(scope.Context, ParallelStep);

            newScope.SetStatus(ProcessingStatus.Running, )

#pragma warning disable CS4014


            scope.Context.Scopes.Add();

#pragma warning restore CS4014

            return await base.OnExecution(scope);
        }
    }
}
