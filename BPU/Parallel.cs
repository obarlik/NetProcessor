using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BPU
{
    public class Parallel : ProcessStep
    {
        public ProcessStep ParallelStep;

        protected override async Task<ProcessStep> Process(Scope scope)
        {
#pragma warning disable CS4014
            ParallelStep.Execute(scope.Clone());
#pragma warning restore CS4014

            return await base.Process(scope);
        }
    }
}
