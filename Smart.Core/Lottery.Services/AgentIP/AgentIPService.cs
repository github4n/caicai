using System;
using System.Collections.Generic;
using System.Text;
using log4net;
using Lottery.Modes.Model;
using Lottery.Services.Abstractions;
using Smart.Core.Repository.SqlSugar;
using SqlSugar;

namespace Lottery.Services
{
    public class AgentIPService : Repository<DbFactory>, IAgentIPService
    {
        protected SqlSugarClient db = null;
        private ILog log;
        public AgentIPService(DbFactory factory) : base(factory)
        {
            db = factory.GetDbContext();
            log = LogManager.GetLogger("LotteryRepository", typeof(AgentIPService));
        }
        public void AddAgentIPList(List<IP> iPs, out int Count)
        {
            int i = 0;
            iPs.ForEach((a) =>
            {
                bool b = db.Queryable<IP>().Where(x => x.IPAddress == a.IPAddress && x.Port == a.Port).First() == null ? true : false;
                if (b)
                {
                    db.Insertable<IP>(a).ExecuteCommand();
                    i++;
                }
            });
            Count = i;
        }
        public void DeleteNotUseAgentIP(int id)
        {
            var model = db.Queryable<IP>().Where(x => x.ID == id).First();
            if (model == null) return;
            if (model.FailNum + 1 >= 3)
            {
                model.FailNum = model.FailNum + 1;
                model.IsDelete = true;
                db.Updateable(model).UpdateColumns(x => new { x.IsDelete,x.FailNum }).ExecuteCommand();
            }
            else
            {
                model.FailNum = model.FailNum + 1;
                db.Updateable(model).UpdateColumns(x => new { x.FailNum }).ExecuteCommand();
            }
        }
        public List<IP> GetIPs()
        {
            return db.Queryable<IP>().Where(x=>x.IsDelete==false).OrderBy(x=>x.Speed,OrderByType.Asc).ToList();
        }
    }
}
