using System.Collections.Generic;

namespace BPU
{
    public interface ProcessProvider
    {
        IEnumerable<Process> GetProcesses();

        Process NewProcess();

        void SaveProcess(Process process);
    }
}