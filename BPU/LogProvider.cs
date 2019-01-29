using System;
using System.Threading.Tasks;

namespace BPU
{
    public abstract class LogProvider
    {
        public LogProvider()
        {
        }


        public virtual async Task AddLog(Log log)
        {
            await Task.FromResult(true);
        }


        public async Task AddLog(string hostName, Guid? contextId, Guid? scopeId, string message, params object[] prms)
        {
            await AddLog(new Log(hostName, contextId, scopeId, message, prms));
        }      
    }
}