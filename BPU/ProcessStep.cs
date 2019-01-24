using System;
using System.Collections.Generic;
using System.Text;

namespace BPU
{
    public class ProcessStep
    {
        public Process ParentProcess;
        public string Name;
        public ProcessStep NextStep;

        public T Next<T>(T Step) where T : ProcessStep
        {
            NextStep = Step;
            return Step;
        }

        public virtual ProcessStep Process(Context context)
        {
            return NextStep;
        }
    }
}
