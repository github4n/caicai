using System;
using System.Collections.Generic;
using System.Text;
using Lottery.Modes.Entity;
using Lottery.Services.Abstractions;
using Smart.Core.Repository.SqlSugar;
using SqlSugar;
using System.Threading.Tasks;

namespace Lottery.Services
{
   public class BJDC_DataService: Repository<DbFactory>, IBJDC_DataService
    {
        protected SqlSugarClient db = null;
        public BJDC_DataService(DbFactory factory) : base(factory)
        {
            db = factory.GetDbContext();
        }
        public void AddBJDCAsync(List<bjdc_result> model)
        {
           db.Insertable(model);
        }
    }
}
