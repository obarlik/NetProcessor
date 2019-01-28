using System;

namespace BPU
{
    public class Log
    {
        public Scope Scope;
        public DateTimeOffset Time;
        public string Message;


        public Log()
        {
        }


        public Log(Scope scope, string message, params object[] prms)
        {
            Time = DateTimeOffset.Now;
            Message = prms.Length > 0 ? string.Format(message, prms) : message;
        }
    }
}