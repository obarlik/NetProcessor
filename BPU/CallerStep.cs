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
            if (scope.Status == ProcessingStatus.Running)
            {
                return await Task.Run(() =>
                {
                    var calledScope = scope.NewScope();

                    calledScope.CallerScope = scope;
                    calledScope.CurrentStep = CalledProcess.Steps.First(s => s is StartStep);

                    scope.Status = ProcessingStatus.AwaitingReturn;
                    scope.StatusMessage = "Awaiting called process '" + CalledProcess.Name + "'";

                });
            }

            return  (ProcessStep)null;

        }

    }
}
