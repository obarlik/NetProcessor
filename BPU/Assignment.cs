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
                scope.SetVariable(Variable, Expression.Compile()(scope));
                
                return NextStep;
            });
        }
    }
}
