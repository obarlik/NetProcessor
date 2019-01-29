using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BPU
{
    public class CallerStep : ProcessStep
    {
        public ProcessStep CalledStep;

        protected override async Task<ProcessStep> Process(Scope scope)
        {
            if (scope.Status == ProcessingStatus.Running)
            {
                if (CalledStep == null)
                    return await Task.FromResult(NextStep);

                var calledScope = scope.NewScope();

                calledScope.CallerScope = scope;
                calledScope.CurrentStep = CalledProcess.Steps.First(s => s is StartStep);

                scope.Status = ProcessingStatus.AwaitingReturn;
                scope.StatusMessage = "Awaiting called process '" + CalledProcess.Name + "'";
            }

            return await Task.FromResult<ProcessStep>(null);
        }

    }
}
