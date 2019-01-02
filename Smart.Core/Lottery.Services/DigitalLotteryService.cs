using System;
using System.Collections.Generic;
using System.Text;
using EntityModel.Model;
using Lottery.Modes.Entity;
using Lottery.Services.Abstractions;
using Smart.Core.Repository.SqlSugar;
using SqlSugar;
using Newtonsoft.Json;
using System.Linq;

namespace Lottery.Services
{
    public class DigitalLotteryService : Repository<DbFactory>, IDigitalLotteryService
    {
        protected SqlSugarClient db = null;
        public DigitalLotteryService(DbFactory factory) : base(factory)
        {
            db = factory.GetDbContext();
        }
        public void Addnormal_lotterydetail(List<fc3D> ModelList)
        {
            var LotteryCode = db.Queryable<sys_lottery>().Where(x => x.LotteryCode == "sd").First();
            if (LotteryCode == null)
            {
                return;
            }
            List<normal_lotterydetail> result = new List<normal_lotterydetail>();
            int i = 0;
            string Issue = string.Empty;
            ModelList.ForEach((a) =>
            {
                bool b = db.Queryable<normal_lotterydetail>().Where(x => x.LotteryCode == LotteryCode.LotteryCode && x.IssueNo == a.expect).Count() > 0 ? true : false;
                if (!b)
                {
                    normal_lotterydetail _Lotterydetail = new normal_lotterydetail
                    {
                        LotteryId = LotteryCode.Lottery_Id,
                        LotteryCode = LotteryCode.LotteryCode,
                        IssueNo = a.expect,
                        LotteryDataDetail = JsonConvert.SerializeObject(a.SubItemList),
                        LotteryResultDetail = a.opencode + "|" + a.TestNumber + "|" + a.numberType,
                        OpenTime = a.LotteryDate,
                        AwardDeadlineTime = a.AwardDeadline,
                        CurrentSales = a.SalesVolume
                    };
                    db.Insertable(_Lotterydetail);
                    i++;
                    if (string.IsNullOrEmpty(Issue))
                    {
                        Issue = a.expect;
                    }
                }
            });
            Console.WriteLine($"FC3D奖期{Issue}成功新增数据{i}条");
        }

        public void AddZqdc_Sfgg(List<zqdc_sfgg_result> zqdc_Sfggs)
        {
            int u = 0, i = 0;
            string issUe = zqdc_Sfggs.Select(x => x.IssueNo).First();
            zqdc_Sfggs.ForEach((x) => {
                x.MatchId = x.IssueNo + x.MatchNumber;
                x.CreateTime = DateTime.Now;
                var model=db.Queryable<zqdc_sfgg_result>().Where(k => k.IssueNo == x.IssueNo && k.MatchId == x.MatchId).First();
                if (model != null)
                {
                    if (!model.IsFinish.Value)
                    {
                        db.Updateable<zqdc_sfgg_result>(x);
                        u++;
                    }
                }
                else
                {
                    db.Insertable<zqdc_sfgg_result>(x);
                    i++;
                }
            });
            Console.WriteLine($"足球单场胜负过关奖期{issUe}成功新增数据{i}条");
            Console.WriteLine($"足球单场胜负过关奖期{issUe}成功更新数据{u}条");
        }
        public string Getnormal_lotteryIssue()
        {
            return db.Queryable<normal_lotterydetail>().OrderBy(x => x.IssueNo, OrderByType.Desc).Select(x => x.IssueNo).First();
        }
        public string GetZqdc_Sfgg()
        {
            return db.Queryable<zqdc_sfgg_result>().OrderBy(x => x.IssueNo, OrderByType.Desc).Select(x => x.IssueNo).First();
        }
    }
}
