using System;
using System.Collections.Generic;
using System.Text;
using Lottery.Modes.Model;

namespace Lottery.Services.Abstractions
{
   public interface IAgentIPService
    {
        void AddAgentIPList(List<IP> iPs, out int Count);
        void DeleteNotUseAgentIP(int id);
        List<IP> GetIPs();
    }
}
