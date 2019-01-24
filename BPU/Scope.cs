using System;
using System.Collections.Generic;
using System.Text;

namespace BPU
{
    public class Scope : Dictionary<string, object>
    {
        public Context Context;
        public ProcessStep CurrentStep;
        public Scope CallerScope;
        public Dictionary<string, object> Parameters;
        public object Result;
    }
}
