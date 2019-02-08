using System;
using System.Threading.Tasks;

namespace BPU
{
    public interface LogProvider
    {
        void SaveLog(Log log);
    }
}