using System;
using System.Collections.Generic;
using System.Text;

namespace BPU
{
    public class Context : Dictionary<string, object>
    {
        public Scope CurrentScope;
        public List<Log> Logs;
        
        public void AddLog(string message, params object[] prms)
        {
            Logs.Add(new Log()
            {
                Scope = CurrentScope,
                Time = DateTimeOffset.Now,
                Message = prms.Length > 0 ? message : string.Format(message, prms)
            });
        }
    }
}
