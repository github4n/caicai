using HtmlAgilityPack;
using Lottery.Modes.Entity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Lottery.Services.Abstractions
{
   public interface IJddDataService
    {
        Task<int> AddIssue(List<sys_issue> sys_Issues);
    }
}
