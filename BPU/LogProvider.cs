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


        public async Task AddLog(Scope scope, string message, params object[] prms)
        {
            await AddLog(new Log(scope, message, prms));
        }        
    }
}