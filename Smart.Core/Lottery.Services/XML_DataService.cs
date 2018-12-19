using Lottery.Modes.Entity;
using Lottery.Services.Abstractions;
using Smart.Core.Repository.SqlSugar;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace Lottery.Services
{
   public class XML_DataService: Repository<DbFactory>,IXML_DataService
    {
        protected SqlSugarClient db = null;
        public XML_DataService(DbFactory factory) : base(factory)
        {
            db = factory.GetDbContext();
        }

        public void AddGdklsfAsync(XmlNodeList xmlNodeList)
        {
            var lottery = db.Queryable<sys_lottery>().Where(n=>n.LotteryCode== "gdklsf").First();
            foreach (XmlNode item in xmlNodeList)
            {
                sys_issue issue = new sys_issue();
                issue.IssueNo = item.Attributes["expect"].Value;
                issue.OpenCode = item.Attributes["opencode"].Value;
                issue.OpenTime = item.Attributes["opentime"].Value;
               
            }

        }
    }
}
