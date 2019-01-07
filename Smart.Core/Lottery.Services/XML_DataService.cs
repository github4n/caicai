using HtmlAgilityPack;
using log4net;
using Lottery.Modes.Entity;
using Lottery.Services.Abstractions;
using Smart.Core.Repository.SqlSugar;
using SqlSugar;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Lottery.Services
{
   public class XML_DataService: Repository<DbFactory>,IXML_DataService
    {
    
        private ILog log;
        protected DbFactory BaseFactory;
        public XML_DataService(DbFactory factory) : base(factory)
        {
            BaseFactory = factory;
            log = LogManager.GetLogger("LotteryRepository", typeof(XML_DataService));
        }
       


        public async Task<int> AddXMLAsync(XmlNodeList xmlNodeList,string gameCode,string LotteryTime)
        {
            using (var db = BaseFactory.GetDbContext())
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
                    log.Info(lottery.LotteryName+"彩种"+issue.IssueNo+ "期期号采集完毕");
                }
                if (insertObjs.Count != 0)
                {
                    count = db.Insertable(insertObjs).ExecuteCommand();
                    //await db.InsertRange(insertObjs);
                }

                return await Task.FromResult(count);
            }
        }

        public async Task<int> AddQGDFCXMLAsync(XmlNodeList xmlNodeList, string gameCode)
        {
            using (var db = BaseFactory.GetDbContext())
            {
                int count = 0;
                int insertCount = 0;
                var lottery = GetLottery(gameCode);
                sys_issue sys_issue = GetNowIssuNo(gameCode);
                //ArrayList arrNodeList = new ArrayList();
                //foreach (XmlNode item in xmlNodeList)
                //{
                //    arrNodeList.Add(item);
                //}
                //arrNodeList.Reverse();
                foreach (XmlNode item in xmlNodeList)
                {



                    if (sys_issue != null && gameCode != "sfc")
                    {

                        if (sys_issue.IssueNo == item.Attributes["expect"].Value)
                        {
                            break;
                        }
                    }
                    var strCI = GetCodeIssue(gameCode, item.Attributes["expect"].Value);

                    if (strCI != null)
                    {
                        if (gameCode == "sfc")
                        {
                            DateTime date = DateTime.Now.AddDays(-3);
                            if (strCI.OpenCode.Contains("*") && Convert.ToDateTime(strCI.OpenTime)> date)
                            {
                                strCI.OpenCode = item.Attributes["opencode"].Value;
                                strCI.UpdateTime = DateTime.Now;
                                count = db.Updateable(strCI).ExecuteCommand();
                                log.Info(lottery.LotteryName + "彩种" + strCI.IssueNo + "期期号更新完毕");
                            }
                        }
                    }
                    else
                    {
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
                        count = db.Insertable(issue).ExecuteCommand();
                        log.Info(lottery.LotteryName + "彩种" + issue.IssueNo + "期期号采集完毕");
                        insertCount += count;
                    }
                }
                
                return await Task.FromResult(insertCount);
            }
        }


        public async Task<int> AddBjdcIssue(HtmlNodeCollection htmlNodeCollection, string gameCode)
        {
            using (var db = BaseFactory.GetDbContext())
            {
                int count = 0;
                var lottery = GetLottery(gameCode);
                var insertObjs = new List<sys_issue>();
                sys_issue sys_issue = GetDescIssuNo(gameCode);
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
                    log.Info(lottery.LotteryName + "彩种" + issue.IssueNo + "期期号采集完毕");
                }
                if (insertObjs.Count != 0)
                {
                    count = db.Insertable(insertObjs.ToArray()).ExecuteCommand();

                }

                return await Task.FromResult(count);
            }
        }

        public async Task<int> AddSDhtml(List<sys_issue> sys_Issues, string gameCode)
        {
            using (var db = BaseFactory.GetDbContext())
            {
                int count = 0;
                int insertCount = 0;
                var lottery = GetLottery(gameCode);
                sys_issue sys_issue = GetNowIssuNo(gameCode);
                foreach (var item in sys_Issues)
                {
                    if (sys_issue != null)
                    {
                        if (sys_issue.IssueNo == item.IssueNo)
                        {
                            break;
                        }
                    }
                    sys_issue issue = new sys_issue();
                    issue.IssueNo = item.IssueNo;

                    issue.OpenCode = item.OpenCode;


                    issue.OpenTime = item.OpenTime;
                    issue.LotteryId = lottery.Lottery_Id;
                    issue.LotteryCode = lottery.LotteryCode;
                    issue.CreateTime = DateTime.Now;
                    issue.UpdateTime = DateTime.Now;
                    count = db.Insertable(issue).ExecuteCommand();
                    log.Info(lottery.LotteryName + "彩种" + issue.IssueNo + "期期号采集完毕");
                    insertCount += count;
                }
                return await Task.FromResult(insertCount);
            }
        }
        /// <summary>
        /// 获取彩种
        /// </summary>
        /// <param name="LotteryCode"></param>
        /// <returns></returns>
        public sys_lottery GetLottery(string LotteryCode)
        {
            using (var db = BaseFactory.GetDbContext())
            {
                sys_lottery lottery = db.Queryable<sys_lottery>().Where(n => n.LotteryCode == LotteryCode).First();
                return lottery;
            }
        }

        /// <summary>
        /// 根据彩种，采集日期并且期号降序最新信息
        /// </summary>
        /// <returns></returns>
        public sys_issue GetNowGpcIssuNo(string LotteryCode,string LotteryTime)
        {
            using (var db = BaseFactory.GetDbContext())
            {
                sys_issue issue = db.Queryable<sys_issue>().Where(n => n.LotteryCode == LotteryCode && n.LotteryTime == LotteryTime).OrderBy(n => n.IssueNo, OrderByType.Desc).Take(1).First();
                return issue;
            }
            
        }
        /// <summary>
        /// 根据彩种并且开奖时间降序最新信息
        /// </summary>
        /// <returns></returns>
        public sys_issue GetNowIssuNo(string LotteryCode)
        {
            using (var db = BaseFactory.GetDbContext())
            {
                sys_issue issue = db.Queryable<sys_issue>().Where(n => n.LotteryCode == LotteryCode).OrderBy(n => n.OpenTime, OrderByType.Desc).Take(1).First();
                return issue;
                
            }

        }

        /// <summary>
        /// 根据彩种并且期号降序获取最新期号
        /// </summary>
        /// <param name="LotteryCode"></param>
        /// <returns></returns>
        public sys_issue GetDescIssuNo(string LotteryCode)
        {
            using (var db = BaseFactory.GetDbContext())
            {
                sys_issue issue = db.Queryable<sys_issue>().Where(n => n.LotteryCode == LotteryCode).OrderBy(n => n.IssueNo, OrderByType.Desc).Take(1).First();
                return issue;
            }
        }

        public sys_issue GetCodeIssue(string LotteryCode,string IssueNo)
        {
            using (var db = BaseFactory.GetDbContext())
            {
                return db.Queryable<sys_issue>().Where(n=>n.LotteryCode==LotteryCode && n.IssueNo==IssueNo).OrderBy(n => n.IssueNo, OrderByType.Desc).First();
            }
        }

        public List<sys_lottery> GetHighFrequency()
        {
            using (var db = BaseFactory.GetDbContext())
            {
                return db.Queryable<sys_lottery>().ToList();
            }
        }

    }
}
