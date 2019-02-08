using System;
using System.Collections.Generic;
using System.Text;

namespace BPU
{
    public class LogEventArgs : EventArgs
    {
        public LogEventArgs(Log log)
        {
            Log = log;
        }

        public LogEventArgs(string hostName, Guid? contextId, Guid scopeId, string message) 
            : this(new Log(hostName, contextId, scopeId, message))
        {
        }

        public Log Log { get; set; }
    }
}
