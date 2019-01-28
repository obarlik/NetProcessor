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
        
        protected override async Task<ProcessStep> Process(Scope scope)
        {
            return await Task.Run(() =>
            {
                scope.Result = Expression.Compile()(scope);

                if (scope.ContainsKey(Variable))
                {
                    scope[Variable] = scope.Result;
                }
                else if (scope.Context.ContainsKey(Variable))
                {
                    scope.Context[Variable] = scope.Result;
                }
                else if (scope.Context.Host.ContainsKey(Variable))
                {
                    scope.Context.Host[Variable] = scope.Result;
                }

                return NextStep;
            });
        }
    }
}
