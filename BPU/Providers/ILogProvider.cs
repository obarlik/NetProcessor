using BPU.Core;
using System;
using System.Threading.Tasks;

namespace BPU
{
    public interface ILogProvider
    {
        LogLevel CurrentLogLevel { get; set; }
        void OnLog(object sender, LogLevel level, string message);
    }
}