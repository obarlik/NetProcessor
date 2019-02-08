using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BPU
{
    public class AssignmentStep : ProcessStep
    {
        public string Variable;
        public Expression<Func<Scope, object>> Expression;

        protected override ProcessStep OnExecution(Scope scope)
        {
            scope.SetVariable(Variable, Expression.Compile()(scope));

            return NextStep;
        }
    }
}
