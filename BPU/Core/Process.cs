using System;
using System.Collections.Generic;

namespace BPU
{
    public class Process
    {
        public Guid ProcessId;

        public string Name;

        public Dictionary<Guid, ProcessStep> Steps { get; private set; }


        public Process()
        {
            Steps = new Dictionary<Guid, ProcessStep>();
        }


        public T AddStep<T>(Action<T> initializer) where T : ProcessStep, new()
        {
            var step = new T();
            Steps.Add(step.ProcessStepId, step);
            initializer?.Invoke(step);
            return step;
        }
    }
}
