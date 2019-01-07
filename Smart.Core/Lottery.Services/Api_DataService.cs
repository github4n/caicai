using Lottery.Modes.Entity;
using Lottery.Services.Abstractions;
using Smart.Core.Repository.SqlSugar;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Smart.Core.Throttle;
using Lottery.Modes.ShowModel;
using Smart.Core.NoSql.Redis;
using Smart.Core.Extensions;

namespace Lottery.Services
{
    public class Api_DataService : Repository<DbFactory>, IApi_DataService
    {
        //protected SqlSugarClient db = null;
        protected DbFactory basFactory = null;
        public Api_DataService(DbFactory factory) : base(factory)
        {
            //db = factory.GetDbContext();
            basFactory = factory;
        }

        /// <summary>
        /// 获取地区列表
        /// </summary>
        /// <returns></returns>
        public List<sys_region> GetAreaList()
        {
            using (var db = basFactory.GetDbContext())
            {
                return db.Queryable<sys_region>().Where(p => p.IsShow == true).ToList();
            }
        }

        /// <summary>
        /// 获取彩种列表
        /// </summary>
        /// <returns></returns>
        public List<sys_lottery> GetAllLottery()
        {
            using (var db = basFactory.GetDbContext())
            {
                return db.Queryable<sys_lottery>().Where(p => p.IsShow == true).ToList();
            }
        }

        /// <summary>
        /// 根据Code获取期号列表
        /// </summary>
        /// <param name="LotteryCode"></param>
        /// <returns></returns>
        public List<sys_issue> GetLotteryIssuesByCode(string LotteryCode)
        {
            using (var db = basFactory.GetDbContext())
            {
                return db.Queryable<sys_issue>().Where(p => p.LotteryCode == LotteryCode).OrderBy(p => p.OpenTime, OrderByType.Desc).Take(100).ToList();
            }
        }

        /// <summary>
        /// 查询高频彩当前彩期列表
        /// </summary>
        /// <returns></returns>
        public List<Issue_ShowModel> GetHighLotteryIssues()
        {
            try
            {
                //弃用sql语句，速度太慢
                //            SELECT* FROM
                //(select i.*from sys_issue i inner
                //           join sys_lottery l on (i.LotteryCode = l.LotteryCode)  where l.HighFrequency = 1
                //ORDER BY i.OpenTime DESC limit 0,100000000)  t
                //GROUP BY t.LotteryCode ORDER BY OpenTime DESC
                var key = "KJ_GetHighLotteryIssues";
                var str = RedisManager.DB_Other.Get(key);
                if (!string.IsNullOrEmpty(str))
                {
                    return JsonHelper.Deserialize<List<Issue_ShowModel>>(str);
                }
                var result = GetHighLotteryList();
                return result;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// 查询地区彩期列表
        /// </summary>
        /// <returns></returns>
        public List<Issue_ShowModel> GetLocalLotteryIssues()
        {
            var key = "KJ_GetLocalLotteryIssues";
            var str = RedisManager.DB_Other.Get(key);
            if (!string.IsNullOrEmpty(str))
            {
                return JsonHelper.Deserialize<List<Issue_ShowModel>>(str);
            }
            var result = GetLocalLotteryList();
            if (result.Count > 0)
            {
                RedisManager.DB_Other.Set(key, result.ToJson(), 60 * 10);
            }
            return result;
        }

        /// <summary>
        /// 查询全国彩彩期列表
        /// </summary>
        /// <returns></returns>
        public List<Issue_ShowModel> GetCountryLotteryIssues()
        {
            var key = "KJ_GetCountryLotteryIssues";
            var str = RedisManager.DB_Other.Get(key);
            if (!string.IsNullOrEmpty(str))
            {
                return JsonHelper.Deserialize<List<Issue_ShowModel>>(str);
            }
            var result = GetCountryLotteryList();
            return result;
        }

        private Issue_ShowModel GetJCZQNewModel()
        {
            var sql = "select * from (select JCDate,Count(*) as TotalCount from jczq_Result group by JCDate) t order by JCDate desc limit 0,1";
            using (var db = basFactory.GetDbContext())
            {
                var obj = db.SqlQueryable<dynamic>(sql).First();
                if (obj == null) return new Issue_ShowModel() { LotteryCode = "jczq", LotteryName = "竞彩足球", OpenTime = DateTime.Now.ToString("yyyy-MM-dd"), OpenCode = "0|0" };
                return new Issue_ShowModel() { LotteryCode = "jczq", LotteryName = "竞彩足球", OpenTime = obj.JCDate, OpenCode = obj.TotalCount + "|" + obj.TotalCount };
            }
        }

        private Issue_ShowModel GetJCLQNewModel()
        {
            var sql = "select * from (select JCDate,Count(*) as TotalCount from jclq_Result group by JCDate) t order by JCDate desc limit 0,1";
            using (var db = basFactory.GetDbContext())
            {
                var obj = db.SqlQueryable<dynamic>(sql).First();
                if (obj == null) return new Issue_ShowModel() { LotteryCode = "jclq", LotteryName = "竞彩篮球", OpenTime = DateTime.Now.ToString("yyyy-MM-dd"), OpenCode = "0|0" };
                return new Issue_ShowModel() { LotteryCode = "jclq", LotteryName = "竞彩篮球", OpenTime = obj.JCDate, OpenCode = obj.TotalCount + "|" + obj.TotalCount };
            }
        }

        private Issue_ShowModel GetZQDCNewModel()
        {
            var sql = "select IssueNo,Count(*) as TotalCount,IsFinish from bjdc_Result  where IssueNo=(select IssueNo from sys_issue where LotteryCode='zqdc' order by issueNo desc limit 0,1) group by IsFinish";
            using (var db = basFactory.GetDbContext())
            {
                var objList = db.SqlQueryable<dynamic>(sql).ToList();
                var result = new Issue_ShowModel() { LotteryCode = "zqdc", LotteryName = "足球单场" };
                long total = 0;
                long finish = 0;
                if (objList.Count > 0)
                {
                    result.IssueNo = objList[0].IssueNo;
                    foreach (var item in objList)
                    {
                        if (item.IsFinish == true)
                        {
                            total = total + item.TotalCount;
                            finish = finish + item.TotalCount;
                        }
                        else
                        {
                            total = total + item.TotalCount;
                        }
                    }
                }
                result.OpenCode = total + "|" + finish;
                return result;
            }
        }

        /// <summary>
        /// 根据时间和彩种拿高频彩数据
        /// </summary>
        /// <param name="LotteryCode"></param>
        /// <param name="LotteryTime"></param>
        /// <returns></returns>
        public List<sys_issue> GetLotteryIssuesByCodeAndTime(string LotteryCode, string LotteryTime)
        {
            //var dt = LotteryTime.ToString("yyyy-MM-dd");
            using (var db = basFactory.GetDbContext())
            {
                return db.Queryable<sys_issue>().Where(p => p.LotteryCode == LotteryCode && p.LotteryTime == LotteryTime).OrderBy(p => p.OpenTime, OrderByType.Desc).ToList();
            }
        }

        /// <summary>
        /// 获取普通彩票详情（除足球篮球及高频彩外）
        /// </summary>
        /// <param name="LotteryCode"></param>
        /// <param name="IssueNo"></param>
        /// <returns></returns>
        public NormalDetail_ShowModel GetLotteryDetail(string LotteryCode, string IssueNo)
        {
            var result = new NormalDetail_ShowModel() { LotteryCode = LotteryCode };
            using (var db = basFactory.GetDbContext())
            {
                if (string.IsNullOrEmpty(IssueNo))
                {
                    var sql = @"SELECT ni.issueNo AS IssueNo,ni.LotteryCode AS LotteryCode,ni.OpenCode AS OpenCode,l.LotteryName AS  LotteryName ,ni.OpenTime
FROM  (SELECT * FROM sys_issue i 
WHERE i.LotteryCode=@LotteryCode
ORDER BY i.OpenTime DESC,i.IssueNo LIMIT 1) ni LEFT JOIN sys_lottery l ON(l.LotteryCode=ni.lotteryCode)";

                    var IssueItem = db.SqlQueryable<NormalDetail_ShowModel>(sql).AddParameters(new { LotteryCode = LotteryCode }).First();
                    if (IssueItem != null)
                    {
                        result.LotteryName = IssueItem.LotteryName;
                        result.LotteryDataDetail = IssueItem.OpenCode;
                        result.IssueNo = IssueItem.IssueNo;
                        result.OpenCode = IssueItem.OpenCode;
                        result.OpenTime = IssueItem.OpenTime.FormatDate();
                        var DetailItem = db.Queryable<normal_lotterydetail>().Where(p => p.IssueNo == IssueItem.IssueNo && p.LotteryCode == LotteryCode).First();
                        if (DetailItem != null)
                        {
                            result.AwardDeadlineTime = DetailItem.AwardDeadlineTime;
                            result.CreateTime = DetailItem.CreateTime;
                            result.CurrentSales = DetailItem.CurrentSales;
                            result.LotteryResultDetail = DetailItem.LotteryResultDetail;
                            result.OpenTime = DetailItem.OpenTime;
                            if (!string.IsNullOrEmpty(DetailItem.LotteryDataDetail))
                                result.LotteryDataDetail = DetailItem.LotteryDataDetail;
                        }
                    }
                }
                else
                {
                    var sql = @"select ni.OpenCode,l.LotteryName as LotteryName,
n.AwardDeadlineTime,n.CurrentSales,n.IssueNo,l.LotteryCode,n.LotteryDataDetail,n.LotteryResultDetail,n.OpenTime,n.PrizePool,ni.openTime as IssueTime
 from  (select * from sys_issue i 
 where i.LotteryCode=@LotteryCode and i.issueNo=@IssueNo
 ) ni left join sys_lottery l on(l.LotteryCode=ni.lotteryCode) left join normal_lotterydetail n
 on (ni.LotteryCode=n.LotteryCode and ni.issueNo=n.IssueNo)";
                    var Item = db.SqlQueryable<NormalDetail_ShowModel>(sql).AddParameters(new { LotteryCode = LotteryCode, IssueNo = IssueNo }).First();
                    if (Item != null)
                    {
                        if (string.IsNullOrEmpty(Item.LotteryDataDetail))
                        {
                            Item.LotteryDataDetail = Item.OpenCode;
                        }
                        if (string.IsNullOrEmpty(Item.OpenTime))
                        {
                            Item.OpenTime = Item.IssueTime.FormatDate();
                        }
                        result = Item;
                    }
                }
                return result;
            }
        }

        /// <summary>
        /// 根据时间获取竞彩足球详情
        /// </summary>
        /// <param name="LotteryDate"></param>
        /// <returns></returns>
        public List<jczq_result> GetJCZQDetail(string LotteryDate)
        {
            //当传入时间为空时则拿最新时间
            using (var db = basFactory.GetDbContext())
            {
                if (string.IsNullOrEmpty(LotteryDate))
                {
                    var LotteryDateItem = db.Queryable<jczq_result>().OrderBy(p => p.JCDate, OrderByType.Desc).First();
                    if (LotteryDateItem == null) return new List<jczq_result>();
                    LotteryDate = LotteryDateItem.JCDate;
                }
                return db.Queryable<jczq_result>().Where(p => p.JCDate == LotteryDate).ToList();
            }
        }

        /// <summary>
        /// 根据时间获取竞彩篮球详情
        /// </summary>
        /// <param name="LotteryDate"></param>
        /// <returns></returns>
        public List<jclq_result> GetJCLQDetail(string LotteryDate)
        {
            //当传入时间为空时则拿最新时间
            using (var db = basFactory.GetDbContext())
            {
                if (string.IsNullOrEmpty(LotteryDate))
                {
                    var LotteryDateItem = db.Queryable<jclq_result>().OrderBy(p => p.JCDate, OrderByType.Desc).First();
                    if (LotteryDateItem == null) return new List<jclq_result>();
                    LotteryDate = LotteryDateItem.JCDate;
                }
                return db.Queryable<jclq_result>().Where(p => p.JCDate == LotteryDate).ToList();
            }
        }

        /// <summary>
        /// 根据期号获取北京单场列表
        /// </summary>
        /// <param name="IssueNo"></param>
        /// <returns></returns>
        public List<bjdc_result> GetZQDCDetail(string IssueNo)
        {
            //当传入时间为空时则拿最新期号
            using (var db = basFactory.GetDbContext())
            {
                if (string.IsNullOrEmpty(IssueNo))
                {
                    var IssueNoItem = db.Queryable<sys_issue>().Where(p => p.LotteryCode == "zqdc").OrderBy(p => p.IssueNo, OrderByType.Desc).First();
                    if (IssueNo == null) return new List<bjdc_result>();
                    IssueNo = IssueNoItem.IssueNo;
                }
                var list = db.Queryable<bjdc_result>().Where(p => p.IssueNo == IssueNo).ToList();
                return list;
            }
        }

        public List<zqdc_sfgg_result> GetZQDCSFGGDetail(string IssueNo)
        {
            //当传入时间为空时则拿最新期号
            using (var db = basFactory.GetDbContext())
            {
                if (string.IsNullOrEmpty(IssueNo))
                {
                    var IssueNoItem = db.Queryable<sys_issue>().Where(p => p.LotteryCode == "zqdcsfgg").OrderBy(p => p.IssueNo, OrderByType.Desc).First();
                    if (IssueNo == null) return new List<zqdc_sfgg_result>();
                    IssueNo = IssueNoItem.IssueNo;
                }
                var list = db.Queryable<zqdc_sfgg_result>().Where(p => p.IssueNo == IssueNo).ToList();
                return list;
            }
        }

        #region 把数据加载到redis

        /// <summary>
        /// 加载高频彩数据
        /// </summary>
        public void AddRedisHighLottery()
        {
            try
            {
                var key = "KJ_GetHighLotteryIssues";
                var result = GetHighLotteryList();
                if (result.Count > 0)
                {
                    RedisManager.DB_Other.Set(key, result.ToJson(), 60 * 10);
                }
            }
            catch (Exception ex)
            {
            }
        }

        /// <summary>
        /// 加载地方彩种数据
        /// </summary>
        public void AddRedisLocalLottery()
        {
            try
            {
                var key = "KJ_GetLocalLotteryIssues";
                var result = GetLocalLotteryList();
                if (result.Count > 0)
                {
                    RedisManager.DB_Other.Set(key, result.ToJson(), 60 * 10);
                }
            }
            catch (Exception ex)
            {
            }
        }

        /// <summary>
        /// 加载全国彩种数据
        /// </summary>
        public void AddRedisCountryLottery()
        {
            try
            {
                var key = "KJ_GetCountryLotteryIssues";
                var result = GetCountryLotteryList();
                if (result.Count > 0)
                {
                    RedisManager.DB_Other.Set(key, result.ToJson(), 60 * 10);
                }
            }
            catch (Exception ex)
            {
            }
        }
        #endregion

        private List<Issue_ShowModel> GetHighLotteryList()
        {
            var highType = (int)HighFrequencyType.High;
            using (var db = basFactory.GetDbContext())
            {
                var lotteryList = db.Queryable<sys_lottery>().Where(p => p.IsShow == true && p.HighFrequency == highType).ToList();
                //db.Close();
                var result = new List<Issue_ShowModel>();
                StringBuilder querysql = new StringBuilder();
                if (lotteryList.Count > 0)
                {
                    foreach (var item in lotteryList)
                    {
                        querysql.Append($"(select * from sys_issue where LotteryCode = '{item.LotteryCode}' order by openTime desc, IssueNo desc limit 0, 1)");
                        querysql.Append("union");
                    }
                    var sql = querysql.ToString();
                    sql = sql.Substring(0, sql.Length - 5);
                    var list = db.SqlQueryable<sys_issue>(sql).ToList();
                    foreach (var item in lotteryList)
                    {
                        var model = list.FirstOrDefault(p => p.LotteryCode == item.LotteryCode);
                        if (model != null)
                        {
                            result.Add(new Issue_ShowModel()
                            {
                                IssueNo = model.IssueNo,
                                LotteryCode = model.LotteryCode,
                                LotteryDay = item.LotteryDay,
                                LotteryId = item.Lottery_Id,
                                NumberPeriods = item.NumberPeriods,
                                OpenCode = model.OpenCode.Trim('|'),
                                OpenTime = model.OpenTime.FormatDate(),
                                RegionName = item.RegionName,
                                LotteryName = item.LotteryName
                            });
                        }
                    }
                }
                return result;
            }
        }

        private List<Issue_ShowModel> GetLocalLotteryList()
        {
            var localType = (int)HighFrequencyType.Local;
            using (var db = basFactory.GetDbContext())
            {
                var lotteryList = db.Queryable<sys_lottery>().Where(p => p.IsShow == true && p.HighFrequency == localType).ToList();
                var result = new List<Issue_ShowModel>();
                StringBuilder querysql = new StringBuilder();
                if (lotteryList.Count > 0)
                {
                    foreach (var item in lotteryList)
                    {
                        querysql.Append($@"(SELECT i.IssueNo,i.LotteryCode,i.OpenCode,i.OpenTime,n.PrizePool,n.CurrentSales
 FROM sys_issue i LEFT JOIN normal_lotterydetail n ON(i.IssueNo = n.IssueNo)
 WHERE i.lotteryCode = '{item.LotteryCode}' ORDER BY i.OpenTime DESC, i.IssueNo DESC LIMIT 0, 1)");
                        querysql.Append("union");
                    }
                    var newsql = querysql.ToString();
                    newsql = newsql.Substring(0, newsql.Length - 5);
                    var list = db.SqlQueryable<Issue_ShowModel>(newsql).ToList();
                    foreach (var item in lotteryList)
                    {
                        var model = list.FirstOrDefault(p => p.LotteryCode == item.LotteryCode);
                        if (model != null)
                        {
                            model.LotteryDay = item.LotteryDay;
                            model.LotteryId = item.Lottery_Id;
                            model.NumberPeriods = item.NumberPeriods;
                            model.RegionName = item.RegionName;
                            model.LotteryName = item.LotteryName;
                            model.OpenTime = model.OpenTime.FormatDate();
                            result.Add(model);
                        }
                    }
                }
                return result;
            }
        }

        private List<Issue_ShowModel> GetCountryLotteryList()
        {
            var sql = @"SELECT i.IssueNo,i.LotteryCode,i.OpenCode,i.OpenTime,n.PrizePool,n.CurrentSales
 FROM sys_issue i LEFT JOIN normal_lotterydetail n ON(i.IssueNo= n.IssueNo) 
WHERE i.lotteryCode = @LotteryCode ORDER BY i.OpenTime DESC,i.IssueNo DESC LIMIT 0,1";
            var localType = (int)HighFrequencyType.Country;
            using (var db = basFactory.GetDbContext())
            {
                var lotteryList = db.Queryable<sys_lottery>().Where(p => p.IsShow == true && p.HighFrequency == localType).ToList();
                //db.Close();
                var result = new List<Issue_ShowModel>();
                if (lotteryList.Count > 0)
                {
                    foreach (var item in lotteryList)
                    {
                        if (item.LotteryCode == "jczq")
                        {
                            result.Add(GetJCZQNewModel());
                            continue;
                        }
                        if (item.LotteryCode == "jclq")
                        {
                            result.Add(GetJCLQNewModel());
                            continue;
                        }
                        if (item.LotteryCode == "zqdc")
                        {
                            result.Add(GetZQDCNewModel());
                            continue;
                        }
                        if (item.LotteryCode == "zqdcsfgg")
                        {
                            continue;
                        }
                        var model = db.SqlQueryable<Issue_ShowModel>(sql).AddParameters(new { LotteryCode = item.LotteryCode }).First();
                        if (model != null)
                        {
                            model.LotteryDay = item.LotteryDay;
                            model.LotteryId = item.Lottery_Id;
                            model.NumberPeriods = item.NumberPeriods;
                            model.RegionName = item.RegionName;
                            model.LotteryName = item.LotteryName;
                            model.OpenTime = model.OpenTime.FormatDate();
                            result.Add(model);
                        }
                    }
                }
                return result;
            }
        }
    }
}
