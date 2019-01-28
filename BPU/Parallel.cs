using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BPU
{
    public class Parallel : ProcessStep
    {
        public ProcessStep ParallelStep;

        public override async Task<ProcessStep> Process(Scope scope)
        {
            await ParallelStep.Process(scope.Clone());
            return await base.Process(scope);
        }
    }
}
