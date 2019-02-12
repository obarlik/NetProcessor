using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BPU
{
    public interface IVariableContainer
    {
        Dictionary<string, object> Variables { get; }
    }
}
