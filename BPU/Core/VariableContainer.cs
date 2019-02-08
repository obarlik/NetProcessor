using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BPU
{
    public interface VariableContainer
    {
        Dictionary<string, object> Variables { get; }
    }
}
