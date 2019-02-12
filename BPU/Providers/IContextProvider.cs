using System;
using System.Collections.Generic;

namespace BPU
{
    public interface IContextProvider
    {
        IEnumerable<Context> GetContexts(); 
    }
}