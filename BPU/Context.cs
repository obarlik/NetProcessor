﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BPU
{
    public class Context : Dictionary<string, object>
    {
        public Host Host;
        public List<Scope> Scopes;
        public ProcessingStatus Status;
        public string StatusMessage;


        public Context(Host host)
        {
            Status = ProcessingStatus.Ready;
            StatusMessage = "Ready";
        }


        public async Task AddLog(Scope scope, string message, params object[] prms)
        {
            await Host.SubSystem.LogProvider.AddLog(scope, message, prms);
        }


        public async Task Execute(Host host)
        {
            Host = host;

            Status = ProcessingStatus.Running;
            StatusMessage = "Running";
            var i = 1;

            while (Status == ProcessingStatus.Running
                && i == 1)
            {
                i = 0;

                foreach (var s in Scopes.Where(s => s.Status == ProcessingStatus.Running))
                {
                    i++;
                    await s.Run(this);
                }
            }

            Status = ProcessingStatus.Finished;
            StatusMessage = "Finished";
        }
    }
}
