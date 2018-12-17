using System;
using System.Collections.Generic;
using SqlSugar;

namespace Smart.Core.Repository.SqlSugar
{
    public interface IDbFactory
    {
        SqlSugarClient GetDbContext(Action<Exception> onErrorEvent);
        SqlSugarClient GetDbContext(Action<string, SugarParameter[]> onExecutedEvent);
        SqlSugarClient GetDbContext(Func<string, SugarParameter[], KeyValuePair<string, SugarParameter[]>> onExecutingChangeSqlEvent);
        SqlSugarClient GetDbContext(Action<string, SugarParameter[]> onExecutedEvent = null, Func<string, SugarParameter[], KeyValuePair<string, SugarParameter[]>> onExecutingChangeSqlEvent = null, Action<Exception> onErrorEvent = null);
    }
}
