using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BPU
{
    public class Decision : ProcessStep
    {
        public Expression<Func<Context, bool>> Expression;
        public ProcessStep TrueStep;
        public bool Reversed;

        public override async Task<ProcessStep> Process(Context context)
        {
            return await Task.Run(() =>
                Expression.Compile()(context) ^ Reversed ?
                    TrueStep :
                    NextStep);
        }
    }
}
