using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace BPU
{
    public class Assignment : ProcessStep
    {
        public string Variable;
        public Expression<Func<Context, object>> Expression;
        public bool IsGlobal;

        public override ProcessStep Process(Context context)
        {
            var result = Expression.Compile()(context);

            if (IsGlobal)
                context[Variable] = result;
            else
                context.CurrentScope[Variable] = result;

            context.CurrentScope.Result = result;

            return NextStep;
        }
    }
}
