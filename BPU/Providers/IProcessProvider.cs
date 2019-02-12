using System;
using System.Collections.Generic;

namespace BPU
{
    public interface IProcessProvider
    {
        IEnumerable<Process> GetProcesses();
        ProcessStep FindStep(Guid stepId);
    }
}