using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BPU
{
    public class CallerStep : ProcessStep
    {
        public ProcessStep CalledStep;

        protected override async Task<ProcessStep> OnExecution(Scope scope)
        {
            if (CalledStep != null)
            {
                scope.ReturnSteps.Push(NextStep);
                return await Task.FromResult(CalledStep);
            }

            return await Task.FromResult(NextStep);
        }
    }
}
