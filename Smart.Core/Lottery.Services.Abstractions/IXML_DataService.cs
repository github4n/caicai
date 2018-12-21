using HtmlAgilityPack;
using Lottery.Modes.Entity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Lottery.Services.Abstractions
{
    public interface IXML_DataService
    {
        Task<int> AddXMLAsync(XmlNodeList xmlNodeList, string gameCode, string LotteryTime);

        sys_issue GetNowIssuNo(string LotteryCode);

        Task<int> AddBjdcIssue(HtmlNodeCollection htmlNodeCollection, string gameCode);

        List<sys_lottery> GetHighFrequency();
    }
}
