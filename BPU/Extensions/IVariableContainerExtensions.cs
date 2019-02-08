using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BPU
{
    public static class IVariableContainerExtensions
    {
        public static string GetKey(this IVariableContainer container, string variableName)
        {
            return container.Variables.Keys
                   .FirstOrDefault(k =>
                        k.Equals(variableName,
                                 StringComparison.InvariantCultureIgnoreCase));
        }


        public static void SetVariable(this IVariableContainer container, string variableName, object value)
        {
            var key = container.GetKey(variableName) ?? variableName;

            lock (container.Variables)
            {
                container.Variables[key] = value;
            }
        }


        public static object GetVariable(this IVariableContainer container, string variableName)
        {
            var key = container.GetKey(variableName);

            if (key == null)
                return null;

            lock (container.Variables)
            {
                return container.Variables[key];
            }
        }
    }
}
