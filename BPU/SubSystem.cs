using System;
using System.Collections.Generic;
using System.Text;

namespace BPU
{
    public class SubSystem
    {
        public LogProvider LogProvider;
        public HostProvider HostProvider;
        public ProcessProvider ProcessProvider;
        public ContextProvider ContextProvider;
        public ScopeProvider ScopeProvider;
    }
}
