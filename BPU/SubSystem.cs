using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BPU
{
    public abstract class SubSystem
    {
        public LogProvider LogProvider;
        public ProcessProvider ProcessProvider;
        public ContextProvider ContextProvider;
    }
}
