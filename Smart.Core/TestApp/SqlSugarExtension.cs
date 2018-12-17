using SqlSugar;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SkyRulerBusiness.Infrastructure
{
    public static class SqlSugarExtension
    {
        public static T First<T>(this ISugarQueryable<T> queryable, T defaultValue)
        {
            T result = queryable.First();
            if (result == null)
            {
                return defaultValue;
            }
            return result;
        }

        public static T First<T>(this ISugarQueryable<T> queryable, Expression<Func<T, bool>> expression, T defaultValue)
        {
            T result = queryable.First(expression);
            if (result == null)
            {
                return defaultValue;
            }
            return result;
        }

        public static T Single<T>(this ISugarQueryable<T> queryable, T defaultValue) where T : class, new()
        {
            T result = queryable.Single();
            if (result == null)
            {
                return defaultValue;
            }
            return result;
        }

        public static T Single<T>(this ISugarQueryable<T> queryable, Expression<Func<T, bool>> expression, T defaultValue) where T : class, new()
        {
            T result = queryable.Single(expression);
            if (result == null)
            {
                return defaultValue;
            }
            return result;
        }

        public static void BeginTran(this SqlSugarClient[] clients)
        {
            foreach (SqlSugarClient client in clients)
            {
                client.Ado.BeginTran();
            }
        }

        public static void BeginTran(this SqlSugarClient[] clients, IsolationLevel iso)
        {
            foreach (SqlSugarClient client in clients)
            {
                client.Ado.BeginTran(iso);
            }
        }

        public static void CommitTran(this SqlSugarClient[] clients)
        {
            foreach (SqlSugarClient client in clients)
            {
                client.Ado.CommitTran();
            }
        }

        public static void RollbackTran(this SqlSugarClient[] clients)
        {
            foreach (SqlSugarClient client in clients)
            {
                try
                {
                    client.Ado.RollbackTran();
                }
                catch (Exception)
                {
                    // ignored
                }
            }
        }
    }
}

