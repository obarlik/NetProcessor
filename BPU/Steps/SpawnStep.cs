using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BPU
{
    public class SpawnStep : ProcessStep
    {
        public ProcessStep StartStep;

        protected override ProcessStep OnExecution(Scope scope)
        {
            Scope.Spawn(scope.Context, StartStep);
            return base.OnExecution(scope);
        }
    }
}
