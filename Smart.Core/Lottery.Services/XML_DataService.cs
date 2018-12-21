﻿using HtmlAgilityPack;
using Lottery.Modes.Entity;
using Lottery.Services.Abstractions;
using Smart.Core.Repository.SqlSugar;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
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
       


        public async Task<int> AddXMLAsync(XmlNodeList xmlNodeList,string gameCode,string LotteryTime)
        {
            int count = 0;
            var lottery = GetLottery(gameCode);
            var insertObjs = new List<sys_issue>();
            sys_issue sys_issue = GetNowGpcIssuNo(gameCode, LotteryTime);
            foreach (XmlNode item in xmlNodeList)
            {
                if (sys_issue != null)
                {
                    if (sys_issue.IssueNo == item.Attributes["expect"].Value)
                    {
                        break;
                    }
                }

                sys_issue issue = new sys_issue();
                issue.IssueNo = item.Attributes["expect"].Value;

                if (item.Attributes["specail"] != null)
                {
                    issue.OpenCode = item.Attributes["opencode"].Value + "|" + item.Attributes["specail"].Value;
                }
                else if (item.Attributes["opencode_specail"] != null)
                {

                    issue.OpenCode = item.Attributes["opencode"].Value + "|" + item.Attributes["opencode_specail"].Value;
                }
                else
                {
                    issue.OpenCode = item.Attributes["opencode"].Value;
                }

                issue.OpenTime = item.Attributes["opentime"].Value;
                issue.LotteryId = lottery.Lottery_Id;
                issue.LotteryCode = lottery.LotteryCode;
                issue.CreateTime = DateTime.Now;
                issue.UpdateTime = DateTime.Now;
                issue.LotteryTime = LotteryTime;
                insertObjs.Add(issue);
            }
            if (insertObjs.Count != 0)
            {
                count = db.Insertable(insertObjs).ExecuteCommand();
                //await db.InsertRange(insertObjs);
            }
         
            return await Task.FromResult(count);
        }


        public async Task<int> AddBjdcIssue(HtmlNodeCollection htmlNodeCollection, string gameCode)
        {
            int count = 0;
            var lottery = GetLottery(gameCode);
            var insertObjs = new List<sys_issue>();
            sys_issue sys_issue = GetNowIssuNo(gameCode);
            foreach (var item in htmlNodeCollection)
            {
                if (sys_issue != null)
                {
                    if (sys_issue.IssueNo == item.Attributes["value"].Value)
                    {
                        break;
                    }

                }
                sys_issue issue = new sys_issue();
                issue.IssueNo = item.Attributes["value"].Value;
                issue.LotteryId = lottery.Lottery_Id;
                issue.LotteryCode = lottery.LotteryCode;
                issue.CreateTime = DateTime.Now;
                issue.UpdateTime = DateTime.Now;
                insertObjs.Add(issue);
            }
            if (insertObjs.Count != 0)
            {
                count = db.Insertable(insertObjs.ToArray()).ExecuteCommand();

            }

            return await Task.FromResult(count);
        }

        /// <summary>
        /// 获取彩种
        /// </summary>
        /// <param name="LotteryCode"></param>
        /// <returns></returns>
        public sys_lottery GetLottery(string LotteryCode)
        {
            sys_lottery lottery = db.Queryable<sys_lottery>().Where(n => n.LotteryCode == LotteryCode).First();
            return lottery;
        }

        /// <summary>
        /// 获取彩种最新信息
        /// </summary>
        /// <returns></returns>
        public sys_issue GetNowGpcIssuNo(string LotteryCode,string LotteryTime)
        {
            sys_issue issue = db.Queryable<sys_issue>().Where(n => n.LotteryCode == LotteryCode && n.LotteryTime==LotteryTime).OrderBy(n => n.IssueNo, OrderByType.Desc).Take(1).First();
            if (issue == null)
            {
                return null;
            }
            else
            {
                return issue;
            }
            
        }
        /// <summary>
        /// 获取彩种最新信息
        /// </summary>
        /// <returns></returns>
        public sys_issue GetNowIssuNo(string LotteryCode)
        {
            sys_issue issue = db.Queryable<sys_issue>().Where(n => n.LotteryCode == LotteryCode).OrderBy(n => n.OpenTime, OrderByType.Desc).Take(1).First();
            if (issue == null)
            {
                return null;
            }
            else
            {
                return issue;
            }

        }


        public List<sys_lottery> GetHighFrequency()
        {
            return db.Queryable<sys_lottery>().Where(n => n.HighFrequency != 0).ToList();
        }

    }
}
