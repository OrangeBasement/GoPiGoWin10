using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoTagGo.Common
{
    public enum ClientCommands
    {
    }

    public enum ServerCommands
    {
    }

    public enum ConnectionStatus
    {
        Idle = 0,
        Connecting,
        Listening,
        Connected,
        Failed = 99
    }
}
