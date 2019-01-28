using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BPU
{
    public class CallerStep : ProcessStep
    {
        public Process CalledProcess;


        protected override Task<ProcessStep> Process(Scope scope)
        {
            var calledScope = new Scope()
            {
                CallerScope = scope,
                CurrentStep = CalledProcess.Steps.First(s => s is StartStep)
            };

            var task = calledScope.Run(scope.Context);

            scope.SetStatus(scope.Context, ProcessingStatus.Ready);
        }

    }
}
