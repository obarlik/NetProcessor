using System;
using System.Collections.Generic;
using System.Text;

namespace BPU.Core
{
    [Flags]
    public enum LogLevel : int
    {
        Trace,
        Information,
        Warning,
        Error,
        FatalError,
        Debug,
    }
}
