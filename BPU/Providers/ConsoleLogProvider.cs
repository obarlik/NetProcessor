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

        
        void LogProvider.SaveLog(Log log)
        {
            Console.WriteLine(
                "{0}  {1}  {2}",
                log.Time,
                log.ScopeId,
                log.Message);
        }
    }
}
