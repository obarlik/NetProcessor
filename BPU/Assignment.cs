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
        
        protected override async Task<ProcessStep> OnExecution(Scope scope)
        {
            return await Task.Run(() =>
            {
                var result = Expression.Compile()(scope);

                if (scope.ContainsKey(Variable))
                {
                    scope[Variable] = result;
                }
                else if (scope.Context.ContainsKey(Variable))
                {
                    scope.Context[Variable] = result;
                }
                else
                    scope["$Result"] = result;

                return NextStep;
            });
        }
    }
}
