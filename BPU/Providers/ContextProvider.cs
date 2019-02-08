using System;
using System.Collections.Generic;

namespace BPU
{
    public interface ContextProvider
    {
        IEnumerable<Context> GetContexts(); 
        Context NewContext();
        void SaveContext(Context context);
    }
}