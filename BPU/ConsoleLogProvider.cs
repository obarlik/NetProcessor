using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BPU
{
    public class ConsoleLogProvider : LogProvider
    {
        public ConsoleLogProvider()
        {
        }


        public override async Task AddLog(Log log)
        {
            await Task.Run(() =>
                Console.WriteLine(
                    "{0:20}  {1:10}  {2:45}",
                    log.Time,
                    log.Scope?.ScopeNumber,
                    log.Message));
        }
    }
}
