using System.Collections.Generic;

namespace BPU
{
    public abstract class ProcessProvider
    {
        public ProcessProvider()
        {
        }

        public abstract IEnumerable<Process> GetProcesses();
    }
}