using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BPU
{
    public class DecisionStep : ProcessStep
    {
        public Expression<Func<Scope, bool>> Expression;
        public ProcessStep TrueStep;

        protected override ProcessStep OnExecution(Scope scope)
        {
            return Expression.Compile()(scope) ?
                TrueStep :
                NextStep;
        }
    }
}
