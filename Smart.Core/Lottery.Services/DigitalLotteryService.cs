using System;
using System.Collections.Generic;
using System.Text;
using EntityModel.Model;
using Lottery.Modes.Entity;
using Lottery.Services.Abstractions;
using Smart.Core.Repository.SqlSugar;
using SqlSugar;
using Newtonsoft.Json;

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
            if (ModelList == null || ModelList.Count < 0)
            {
                return;
            }
            var LotteryCode = db.Queryable<sys_lottery>().Where(x => x.LotteryCode == "sd").First();
            if (LotteryCode == null)
            {
                return;
            }
            List<normal_lotterydetail> result = new List<normal_lotterydetail>();
            int i=0;
            string Issue=string.Empty;
            ModelList.ForEach((a) =>
            {
               bool b=db.Queryable<normal_lotterydetail>().Where(x => x.LotteryCode == LotteryCode.LotteryCode && x.IssueNo == a.expect).Count() > 0 ? true : false;
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
            Console.WriteLine($"FC3D奖期{Issue}成功更新数据{i}条");
        } 
    }
}
