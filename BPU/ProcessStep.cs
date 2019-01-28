using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BPU
{
    public class ProcessStep
    {
        public Process ParentProcess;
        public string Name;
        public ProcessStep NextStep;

        public ProcessingStatus Status { get; private set; }
        public string StatusMessage { get; private set; }

        public T Next<T>(T Step) where T : ProcessStep
        {
            NextStep = Step;
            return Step;
        }


        protected async Task EnterProcess(Scope scope)
        {
            await SetStatus(ProcessingStatus.Running, "Scope {0} running.", ScopeNumber);
        }


        public async void SetStatus(Scope scope, ProcessingStatus status, string message, params object[] prms)
        {
            Status = status;
            StatusMessage = message;
            await scope.Context.Host.SubSystem.LogProvider.AddLog(scope, message, prms);
        }


        protected virtual async Task _Process(Scope scope)
        {
            await Task.FromResult(true);
        }


        public async Task<ProcessStep> Process(Scope scope)
        {
            await EnterProcess(scope);
            await _Process(scope);
            return await ExitProcess(scope, NextStep);
        }


        protected Task<ProcessStep> ExitProcess(Scope scope, ProcessStep nextStep)
        {
            await SetStatus(ProcessingStatus.Finished, "Scope {0} done.", ScopeNumber);
            return await Task.FromResult(nextStep);
        }
    }
}
