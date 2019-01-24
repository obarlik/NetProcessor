using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace BPU
{
    public class Decision : ProcessStep
    {
        public Expression<Func<Context, bool>> Expression;
        public ProcessStep TrueStep;
        public bool Reversed;

        public override ProcessStep Process(Context context)
        {
            return Expression.Compile()(context) ^ Reversed ?
                TrueStep :
                NextStep;
        }
    }
}
