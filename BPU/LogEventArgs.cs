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

        public Log Log { get; set; }
    }
}
