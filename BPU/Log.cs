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


        public Log(string hostName, Guid? contextId, Guid? scopeId, string message)
        {
            HostName = hostName;
            ContextId = contextId;
            ScopeId = scopeId;
            Time = DateTimeOffset.Now;
            Message = message;
        }
    }
}