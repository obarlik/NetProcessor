using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using BPU.Core;

namespace BPU
{
    public class ConsoleLogProvider : ILogProvider
    {
        public ConsoleLogProvider()
        {
        }


        public LogLevel CurrentLogLevel;


        public void OnLog(object sender, LogLevel level, string message)
        {
            if ((level & CurrentLogLevel) == level)
                Console.WriteLine(
                    "{0}  {1}  {2}",
                    DateTime.UtcNow,
                    sender is Host ? $"Host: {((Host)sender).Name}" : "",
                    sender is Context ? $"Context Id: {((Context)sender).ContextId}" : "",
                    sender is Scope ? $"Scope Id: {((Scope)sender).ScopeId}" : "",
                    message);
        }
    }
}
