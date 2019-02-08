using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BPU
{
    public class ReturnStep : ProcessStep
    {
        protected override ProcessStep OnExecution(Scope scope)
        {
            if (scope.ReturnSteps.Count > 0)
            {
                return scope.ReturnSteps.Pop();
            }

            return NextStep;
        }
    }
}
