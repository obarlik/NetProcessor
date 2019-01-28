using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BPU
{
    public class Decision : ProcessStep
    {
        public Expression<Func<Scope, bool>> Expression;
        public ProcessStep TrueStep;

        protected override async Task<ProcessStep> Process(Scope scope)
        {
            return await Task.Run(() =>
                Expression.Compile()(scope) ?
                    TrueStep :
                    NextStep);
        }
    }
}
