using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BPU
{
    public class ReturnStep : ProcessStep
    {
        protected override async Task<ProcessStep> OnExecution(Scope scope)
        {
            if (scope.ReturnSteps.Count > 0)
            {
                return await Task.FromResult(scope.ReturnSteps.Pop());
            }

            return await Task.FromResult(NextStep);
        }
    }
}
