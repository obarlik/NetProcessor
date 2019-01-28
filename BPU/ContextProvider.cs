using System.Collections.Generic;

namespace BPU
{
    public abstract class ContextProvider
    {
        public ContextProvider()
        {
        }

        public abstract IEnumerable<Context> GetContexts(Host host);
    }
}