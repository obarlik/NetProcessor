using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BPU
{
    public class Assignment : ProcessStep
    {
        public string Variable;
        public Expression<Func<Scope, object>> Expression;
        
        public override async Task<ProcessStep> Process(Scope scope)
        {
            var t = await base.Process(scope);
            var result = Expression.Compile()(scope);

            if (scope.ContainsKey(Variable))
                scope[Variable] = result;
            else if (scope.Context.ContainsKey(Variable))
                scope.Context[Variable] = result;
            else if (scope.Context.Host.ContainsKey(Variable))
                scope.Context.Host[Variable] = result;

            context.CurrentScope.Result = result;

            return NextStep;
        }
    }
}
