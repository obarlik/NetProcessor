using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BPU
{
    public class VariableDictionary : Dictionary<string, object>
    {
        public VariableDictionary()
        {
        }

        
        public string GetKey(string variableName)
        {
            return Keys.FirstOrDefault(k => k.Equals(variableName, StringComparison.InvariantCultureIgnoreCase));
        }


        public virtual void SetVariable(string variableName, object value)
        {
            this[GetKey(variableName) ?? variableName] = value;
        }


        public virtual object GetVariable(string variableName)
        {
            var key = GetKey(variableName);

            return key != null ? this[key] : null;
        }
    }
}
