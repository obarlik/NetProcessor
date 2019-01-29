using System;

namespace BPU
{
    public class Log
    {
        public string HostName;
        public Guid? ContextId;
        public Guid? ScopeId;
        public DateTimeOffset Time;
        public string Message;


        public Log()
        {
        }


        public Log(string hostName, Guid? contextId, Guid? scopeId, string message, params object[] prms)
        {
            HostName = hostName;
            ContextId = contextId;
            ScopeId = scopeId;
            Time = DateTimeOffset.Now;
            Message = prms.Length > 0 ? string.Format(message, prms) : message;
        }
    }
}