using BPU.Core;
using System;
using System.Threading.Tasks;

namespace BPU
{
    public interface ILogProvider
    {
        void OnLog(object sender, LogLevel level, string message);
    }
}