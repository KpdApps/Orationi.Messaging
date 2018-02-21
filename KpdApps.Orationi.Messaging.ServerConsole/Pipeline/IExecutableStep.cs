using System;
using System.Collections.Generic;
using System.Text;

namespace KpdApps.Orationi.Messaging.ServerConsole.Pipeline
{
    public interface IExecutableStep
    {
        bool IsAsynchronous { get; }
        int Order { get; }
        void Execute();
    }
}
