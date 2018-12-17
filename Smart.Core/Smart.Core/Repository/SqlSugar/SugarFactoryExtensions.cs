using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Smart.Core.Repository.SqlSugar
{
    public static class SugarFactoryExtensions
    {

        //public static DbResult<T> TranInvok<T>(this SqlSugarClient db, Func<T> func)
        /// <summary>
        /// 事务处理
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="db"></param>
        /// <param name="func"></param>
        public static void UseTransaction<T>(this SqlSugarClient db, Func<T> func)
        {
            try
            {
                db.Ado.BeginTran();
                func();
                db.Ado.CommitTran();
            }
            catch (Exception ex)
            {
                db.Ado.RollbackTran();
            }
            //return dbResult;
        }

        /// <summary>
        /// 功能描述:根据ID查询一条数据
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="db"></param>
        /// <param name="objId"></param>
        /// <returns></returns>
        public static async Task<TEntity> QueryByID<TEntity>(this SqlSugarClient db, object objId)
        {
            return await Task.Run(() => db.Queryable<TEntity>().InSingle(objId));
        }

        /// <summary>
        /// 功能描述:根据ID查询一条数据
        /// </summary>
        /// <param name="objId">id（必须指定主键特性 [SugarColumn(IsPrimaryKey=true)]），如果是联合主键，请使用Where条件</param>
        /// <param name="blnUseCache">是否使用缓存</param>
        /// <returns>数据实体</returns>
        public static async Task<TEntity> QueryByID<TEntity>(this SqlSugarClient db, object objId, bool blnUseCache = false)
        {
            return await Task.Run(() => db.Queryable<TEntity>().WithCacheIF(blnUseCache).InSingle(objId));
        }



        /// <summary>
        /// 根据条件获取单个实体对象
        /// </summary>
        /// <typeparam name="TSource">数据源类型</typeparam>
        /// <param name="db"></param>
        /// <param name="whereExp"></param>
        /// <returns></returns>
        public static async Task<TEntity> GetAsync<TEntity>(this SqlSugarClient db, Expression<Func<TEntity, bool>> whereExp) where TEntity : EntityBase, new()
        {
            return await Task.Run(() => db.Queryable<TEntity>().Where(whereExp).Single());
        }

        /// <summary>
        /// 功能描述:根据ID查询数据
        /// </summary>
        /// <param name="lstIds">id列表（必须指定主键特性 [SugarColumn(IsPrimaryKey=true)]），如果是联合主键，请使用Where条件</param>
        /// <returns>数据实体列表</returns>
        public static async Task<List<TEntity>> QueryByIDs<TEntity>(this SqlSugarClient db, object[] lstIds)
        {
            return await Task.Run(() => db.Queryable<TEntity>().In(lstIds).ToList());
        }

        /// <summary>
        /// 写入实体数据
        /// </summary>
        /// <param name="entity">博文实体类</param>
        /// <returns></returns>
        public static async Task<bool> Add<TEntity>(this SqlSugarClient db, TEntity entity) where TEntity : EntityBase, new()
        {
            return await Task.Run(() => db.Insertable(entity).ExecuteCommand() > 0);
        }

        /// <summary>
        /// 更新实体数据
        /// </summary>
        /// <param name="entity">博文实体类</param>
        /// <returns></returns>
        public static async Task<bool> Update<TEntity>(this SqlSugarClient db, TEntity entity) where TEntity : EntityBase, new()
        {
            //这种方式会以主键为条件
            var i = await Task.Run(() => db.Updateable(entity).ExecuteCommand());
            return i > 0;
        }

        public static async Task<bool> Update<TEntity>(this SqlSugarClient db, TEntity entity, string strWhere) where TEntity : EntityBase, new()
        {
            return await Task.Run(() => db.Updateable(entity).Where(strWhere).ExecuteCommand() > 0);
        }

        public static async Task<bool> Update<TEntity>(this SqlSugarClient db, string strSql, SugarParameter[] parameters = null) where TEntity : EntityBase, new()
        {
            return await Task.Run(() => db.Ado.ExecuteCommand(strSql, parameters) > 0);
        }

        public static async Task<bool> Update<TEntity>(
         this SqlSugarClient db,
         TEntity entity,
         List<string> lstColumns = null,
         List<string> lstIgnoreColumns = null,
         string strWhere = ""
           ) where TEntity : EntityBase, new()
        {
            IUpdateable<TEntity> up = await Task.Run(() => db.Updateable(entity));
            if (lstIgnoreColumns != null && lstIgnoreColumns.Count > 0)
            {
                up = await Task.Run(() => up.IgnoreColumns(it => lstIgnoreColumns.Contains(it)));
            }
            if (lstColumns != null && lstColumns.Count > 0)
            {
                up = await Task.Run(() => up.UpdateColumns(it => lstColumns.Contains(it)));
            }
            if (!string.IsNullOrEmpty(strWhere))
            {
                up = await Task.Run(() => up.Where(strWhere));
            }
            return await Task.Run(() => up.ExecuteCommand()) > 0;
        }


        /// <summary>
        /// 根据实体删除一条数据
        /// </summary>
        /// <param name="entity">博文实体类</param>
        /// <returns></returns>
        public static async Task<bool> Delete<TEntity>(this SqlSugarClient db, TEntity entity) where TEntity : EntityBase, new()
        {
            var i = await Task.Run(() => db.Deleteable(entity).ExecuteCommand());
            return i > 0;
        }

        /// <summary>
        /// 删除指定ID的数据
        /// </summary>
        /// <param name="id">主键ID</param>
        /// <returns></returns>
        public static async Task<bool> DeleteById<TEntity>(this SqlSugarClient db, object id) where TEntity : EntityBase, new()
        {
            var i = await Task.Run(() => db.Deleteable<TEntity>(id).ExecuteCommand());
            return i > 0;
        }

        /// <summary>
        /// 删除指定ID集合的数据(批量删除)
        /// </summary>
        /// <param name="ids">主键ID集合</param>
        /// <returns></returns>
        public static async Task<bool> DeleteByIds<TEntity>(this SqlSugarClient db, object[] ids) where TEntity : EntityBase, new()
        {
            var i = await Task.Run(() => db.Deleteable<TEntity>().In(ids).ExecuteCommand());
            return i > 0;
        }



        ///// <summary>
        ///// 功能描述:查询所有数据
        ///// </summary>
        ///// <returns>数据列表</returns>
        //public async Task<List<TEntity>> Query<TEntity>(this SqlSugarClient db)
        //{
        //    return await Task.Run(() => entityDB.GetList());
        //}


        /// <summary>
        /// 功能描述:查询数据列表
        /// </summary>
        /// <param name="strWhere">条件</param>
        /// <returns>数据列表</returns>
        public static async Task<List<TEntity>> Query<TEntity>(this SqlSugarClient db) where TEntity : EntityBase, new()
        {
            return await Task.Run(() => db.Queryable<TEntity>().ToList());
        }

        /// <summary>
        /// 功能描述:查询数据列表
        /// </summary>
        /// <param name="strWhere">条件</param>
        /// <returns>数据列表</returns>
        public static async Task<List<TEntity>> Query<TEntity>(this SqlSugarClient db, string strWhere) where TEntity : EntityBase, new()
        {
            return await Task.Run(() => db.Queryable<TEntity>().WhereIF(!string.IsNullOrEmpty(strWhere), strWhere).ToList());
        }

        /// <summary>
        /// 功能描述:查询数据列表
        /// 作　　者:Blog.Core
        /// </summary>
        /// <param name="whereExpression">whereExpression</param>
        /// <returns>数据列表</returns>
        //public async Task<List<TEntity>> Query(Expression<Func<TEntity, bool>> whereExpression)
        //{
        //    return await Task.Run(() => entityDB.GetList(whereExpression));
        //}

        /// <summary>
        /// 功能描述:查询一个列表
        /// 作　　者:Blog.Core
        /// </summary>
        /// <param name="whereExpression">条件表达式</param>
        /// <param name="strOrderByFileds">排序字段，如name asc,age desc</param>
        /// <returns>数据列表</returns>
        public static async Task<List<TEntity>> Query<TEntity>(this SqlSugarClient db, Expression<Func<TEntity, bool>> whereExpression, string strOrderByFileds) where TEntity : EntityBase, new()
        {
            return await Task.Run(() => db.Queryable<TEntity>().OrderByIF(!string.IsNullOrEmpty(strOrderByFileds), strOrderByFileds).WhereIF(whereExpression != null, whereExpression).ToList());
        }
        /// <summary>
        /// 功能描述:查询一个列表
        /// </summary>
        /// <param name="whereExpression"></param>
        /// <param name="orderByExpression"></param>
        /// <param name="isAsc"></param>
        /// <returns></returns>
        public static async Task<List<TEntity>> Query<TEntity>(this SqlSugarClient db, Expression<Func<TEntity, bool>> whereExpression, Expression<Func<TEntity, object>> orderByExpression, bool isAsc = true) where TEntity : EntityBase, new()
        {
            return await Task.Run(() => db.Queryable<TEntity>().OrderByIF(orderByExpression != null, orderByExpression, isAsc ? OrderByType.Asc : OrderByType.Desc).WhereIF(whereExpression != null, whereExpression).ToList());
        }

        /// <summary>
        /// 功能描述:查询一个列表
        /// 作　　者:Blog.Core
        /// </summary>
        /// <param name="strWhere">条件</param>
        /// <param name="strOrderByFileds">排序字段，如name asc,age desc</param>
        /// <returns>数据列表</returns>
        public static async Task<List<TEntity>> Query<TEntity>(this SqlSugarClient db, string strWhere, string strOrderByFileds) where TEntity : EntityBase, new()
        {
            return await Task.Run(() => db.Queryable<TEntity>().OrderByIF(!string.IsNullOrEmpty(strOrderByFileds), strOrderByFileds).WhereIF(!string.IsNullOrEmpty(strWhere), strWhere).ToList());
        }


        /// <summary>
        /// 功能描述:查询前N条数据
        /// 作　　者:Blog.Core
        /// </summary>
        /// <param name="whereExpression">条件表达式</param>
        /// <param name="intTop">前N条</param>
        /// <param name="strOrderByFileds">排序字段，如name asc,age desc</param>
        /// <returns>数据列表</returns>
        public static async Task<List<TEntity>> Query<TEntity>(
            this SqlSugarClient db,
            Expression<Func<TEntity, bool>> whereExpression,
            int intTop,
            string strOrderByFileds) where TEntity : EntityBase, new()
        {
            return await Task.Run(() => db.Queryable<TEntity>().OrderByIF(!string.IsNullOrEmpty(strOrderByFileds), strOrderByFileds).WhereIF(whereExpression != null, whereExpression).Take(intTop).ToList());
        }

        /// <summary>
        /// 功能描述:查询前N条数据
        /// 作　　者:Blog.Core
        /// </summary>
        /// <param name="strWhere">条件</param>
        /// <param name="intTop">前N条</param>
        /// <param name="strOrderByFileds">排序字段，如name asc,age desc</param>
        /// <returns>数据列表</returns>
        public static async Task<List<TEntity>> Query<TEntity>(
            this SqlSugarClient db,
            string strWhere,
            int intTop,
            string strOrderByFileds) where TEntity : EntityBase, new()
        {
            return await Task.Run(() => db.Queryable<TEntity>().OrderByIF(!string.IsNullOrEmpty(strOrderByFileds), strOrderByFileds).WhereIF(!string.IsNullOrEmpty(strWhere), strWhere).Take(intTop).ToList());
        }



        /// <summary>
        /// 功能描述:分页查询
        /// 作　　者:Blog.Core
        /// </summary>
        /// <param name="whereExpression">条件表达式</param>
        /// <param name="intPageIndex">页码（下标0）</param>
        /// <param name="intPageSize">页大小</param>
        /// <param name="intTotalCount">数据总量</param>
        /// <param name="strOrderByFileds">排序字段，如name asc,age desc</param>
        /// <returns>数据列表</returns>
        public static async Task<List<TEntity>> Query<TEntity>(
            this SqlSugarClient db,
            Expression<Func<TEntity, bool>> whereExpression,
            int intPageIndex,
            int intPageSize,
            string strOrderByFileds) where TEntity : EntityBase, new()
        {
            return await Task.Run(() => db.Queryable<TEntity>().OrderByIF(!string.IsNullOrEmpty(strOrderByFileds), strOrderByFileds).WhereIF(whereExpression != null, whereExpression).ToPageList(intPageIndex, intPageSize));
        }

        /// <summary>
        /// 功能描述:分页查询
        /// 作　　者:Blog.Core
        /// </summary>
        /// <param name="strWhere">条件</param>
        /// <param name="intPageIndex">页码（下标0）</param>
        /// <param name="intPageSize">页大小</param>
        /// <param name="intTotalCount">数据总量</param>
        /// <param name="strOrderByFileds">排序字段，如name asc,age desc</param>
        /// <returns>数据列表</returns>
        public static async Task<List<TEntity>> Query<TEntity>(
          this SqlSugarClient db,
          string strWhere,
          int intPageIndex,
          int intPageSize,
          string strOrderByFileds) where TEntity : EntityBase, new()
        {
            return await Task.Run(() => db.Queryable<TEntity>().OrderByIF(!string.IsNullOrEmpty(strOrderByFileds), strOrderByFileds).WhereIF(!string.IsNullOrEmpty(strWhere), strWhere).ToPageList(intPageIndex, intPageSize));
        }




        public static async Task<List<TEntity>> QueryPage<TEntity>(
            this SqlSugarClient db,
            Expression<Func<TEntity, bool>> whereExpression,
            int intPageIndex = 0, int intPageSize = 20, string strOrderByFileds = null) where TEntity : EntityBase, new()
        {
            return await Task.Run(() => db.Queryable<TEntity>()
            .OrderByIF(!string.IsNullOrEmpty(strOrderByFileds), strOrderByFileds)
            .WhereIF(whereExpression != null, whereExpression)
            .ToPageList(intPageIndex, intPageSize));
        }



        ////////////////////////////////////////////////////////////////////////
        ///


        #region 根据主键获取实体对象

        /// <summary>
        /// 根据主键获取实体对象
        /// </summary>
        /// <typeparam name="TSource">数据源类型</typeparam>
        /// <param name="db"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public static async Task<TSource> GetByIdAsync<TSource>(this SqlSugarClient db, dynamic id) where TSource : EntityBase, new()
        {
            return await Task.Run(() => db.Queryable<TSource>().InSingle(id));
        }

        /// <summary>
        /// 根据主键获取实体对象
        /// </summary>
        /// <typeparam name="TSource">数据源类型</typeparam>
        /// <typeparam name="TMap">数据源映射类型</typeparam>
        /// <param name="db"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public static async Task<TMap> GetById<TSource, TMap>(this SqlSugarClient db, dynamic id) where TSource : EntityBase, new()
        {
            return await Task.Run(() => db.Queryable<TSource>().InSingle(id).MapTo<TMap>());
        }

        #endregion

        #region 根据Linq表达式条件获取单个实体对象

        /// <summary>
        /// 根据条件获取单个实体对象
        /// </summary>
        /// <typeparam name="TSource">数据源类型</typeparam>
        /// <param name="db"></param>
        /// <param name="whereExp"></param>
        /// <returns></returns>
        public static async Task<TSource> Get<TSource>(this SqlSugarClient db, Expression<Func<TSource, bool>> whereExp) where TSource : EntityBase, new()
        {
            return await Task.Run(() => db.Queryable<TSource>().Where(whereExp).Single());
        }

        /// <summary>
        /// 根据条件获取单个实体对象
        /// </summary>
        /// <typeparam name="TSource">数据源类型</typeparam>
        /// <typeparam name="TMap">数据源映射类型</typeparam>
        /// <param name="db"></param>
        /// <param name="whereExp">条件表达式</param>
        /// <returns></returns>
        public static async Task<TMap> Get<TSource, TMap>(this SqlSugarClient db, Expression<Func<TSource, bool>> whereExp) where TSource : EntityBase, new()
        {
            return await Task.Run(() => db.Queryable<TSource>().Where(whereExp).Single().MapTo<TMap>());
        }

        #endregion

        #region 获取所有实体列表

        /// <summary>
        /// 获取所有实体列表
        /// </summary>
        /// <typeparam name="TSource">数据源类型</typeparam>
        /// <param name="db"></param>
        /// <returns></returns>
        public static async Task<List<TSource>> GetList<TSource>(this SqlSugarClient db) where TSource : EntityBase, new()
        {
            return await Task.Run(() => db.Queryable<TSource>().ToList());
        }

        /// <summary>
        /// 获取实体列表
        /// </summary>
        /// <typeparam name="TSource">数据源类型</typeparam>
        /// <typeparam name="TMap">数据源映射类型</typeparam>
        /// <param name="db"></param>
        /// <returns></returns>
        public static async Task<List<TMap>> GetList<TSource, TMap>(this SqlSugarClient db) where TSource : EntityBase, new()
        {
            return await Task.Run(() => db.Queryable<TSource>().ToList().MapTo<TMap>().ToList());
        }

        #endregion

        #region 根据Linq表达式条件获取列表

        /// <summary>
        /// 根据条件获取实体列表
        /// </summary>
        /// <typeparam name="TSource">数据源类型</typeparam>
        /// <param name="db"></param>
        /// <param name="whereExp">条件表达式</param>
        /// <returns></returns>
        public static async Task<List<TSource>> GetList<TSource>(this SqlSugarClient db, Expression<Func<TSource, bool>> whereExp) where TSource : EntityBase, new()
        {
            return await Task.Run(() => db.Queryable<TSource>().Where(whereExp).ToList());
        }

        /// <summary>
        /// 根据条件获取实体列表
        /// </summary>
        /// <typeparam name="TSource">数据源类型</typeparam>
        /// <typeparam name="TMap">数据源映射类型</typeparam>
        /// <param name="db"></param>
        /// <param name="whereExp">条件表达式</param>
        /// <returns></returns>
        public static async Task<List<TMap>> GetList<TSource, TMap>(this SqlSugarClient db, Expression<Func<TSource, bool>> whereExp) where TSource : EntityBase, new()
        {
            return await Task.Run(() => db.Queryable<TSource>().Where(whereExp).ToList().MapTo<TMap>().ToList());
        }

        #endregion

        #region 根据Sugar条件获取列表

        /// <summary>
        /// 根据条件获取实体列表
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="db"></param>
        /// <param name="conditionals">Sugar调价表达式集合</param>
        /// <returns></returns>
        public static async Task<List<TSource>> GetList<TSource>(this SqlSugarClient db, List<IConditionalModel> conditionals) where TSource : EntityBase, new()
        {
            return await Task.Run(() => db.Queryable<TSource>().Where(conditionals).ToList());
        }

        /// <summary>
        /// 根据条件获取实体列表
        /// </summary>
        /// <typeparam name="TSource">数据源类型</typeparam>
        /// <typeparam name="TMap">数据源映射类型</typeparam>
        /// <param name="db"></param>
        /// <param name="conditionals">Sugar调价表达式集合</param>
        /// <returns></returns>
        public static async Task<List<TMap>> GetList<TSource, TMap>(this SqlSugarClient db, List<IConditionalModel> conditionals) where TSource : EntityBase, new()
        {
            return await Task.Run(() => db.Queryable<TSource>().Where(conditionals).ToList().MapTo<TMap>().ToList());
        }

        #endregion

        #region 是否包含某个元素
        /// <summary>
        /// 是否包含某个元素
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="db"></param>
        /// <param name="whereExp">条件表达式</param>
        /// <returns></returns>
        public static async Task<bool> Exist<TSource>(this SqlSugarClient db, Expression<Func<TSource, bool>> whereExp) where TSource : EntityBase, new()
        {
            return await Task.Run(() => db.Queryable<TSource>().Where(whereExp).Any());
        }
        #endregion

        #region 新增实体对象
        /// <summary>
        /// 新增实体对象
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="db"></param>
        /// <param name="insertObj"></param>
        /// <returns></returns>
        public static async Task<bool> Insert<TSource>(this SqlSugarClient db, TSource insertObj) where TSource : EntityBase, new()
        {
            return await Task.Run(() => db.Insertable(insertObj).ExecuteCommand() > 0);
        }

        /// <summary>
        /// 新增实体对象
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TMap"></typeparam>
        /// <param name="db"></param>
        /// <param name="insertDto"></param>
        /// <returns></returns>
        public static async Task<bool> Insert<TSource, TMap>(this SqlSugarClient db, TSource insertDto) where TMap : EntityBase, new()
        {
            var entity = insertDto.MapTo<TMap>();
            return await Task.Run(() => db.Insertable(entity).ExecuteCommand() > 0);
        }
        #endregion

        #region 批量新增实体对象
        /// <summary>
        /// 批量新增实体对象
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="db"></param>
        /// <param name="insertObjs"></param>
        /// <returns></returns>
        public static async Task<bool> InsertRange<TSource>(this SqlSugarClient db, List<TSource> insertObjs) where TSource : EntityBase, new()
        {
            return await Task.Run(() => db.Insertable(insertObjs).ExecuteCommand() > 0);
        }

        /// <summary>
        /// 批量新增实体对象
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TMap"></typeparam>
        /// <param name="db"></param>
        /// <param name="insertObjs"></param>
        /// <returns></returns>
        public static async Task<bool> InsertRange<TSource, TMap>(this SqlSugarClient db, List<TSource> insertObjs) where TMap : EntityBase, new()
        {
            var entitys = insertObjs.MapTo<TMap>().ToList();
            return await Task.Run(() => db.Insertable(entitys).ExecuteCommand() > 0);
        }
        #endregion

        #region 更新单个实体对象
        /// <summary>
        /// 更新单个实体对象
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="db"></param>
        /// <param name="updateObj"></param>
        /// <returns></returns>
        //public static bool Update<TSource>(this SqlSugarClient db, TSource updateObj) where TSource : EntityBase, new()
        //{
        //    return db.Updateable(updateObj).ExecuteCommand() > 0;
        //}
        #endregion

        #region 根据条件批量更新实体指定列
        /// <summary>
        /// 根据条件批量更新实体指定列
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="db"></param>
        /// <param name="columns">需要更新的列</param>
        /// <param name="whereExp">条件表达式</param>
        /// <returns></returns>
        public static async Task<bool> Update<TSource>(this SqlSugarClient db, Expression<Func<TSource, TSource>> columns, Expression<Func<TSource, bool>> whereExp) where TSource : EntityBase, new()
        {
            return await Task.Run(() => db.Updateable<TSource>().UpdateColumns(columns).Where(whereExp).ExecuteCommand() > 0);
        }
        #endregion

        #region 物理删除实体对象

        /// <summary>
        /// 物理删除实体对象
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="db"></param>
        /// <param name="deleteObj"></param>
        /// <returns></returns>
        //public static async Task<bool> Delete<TSource>(this SqlSugarClient db, TSource deleteObj) where TSource : EntityBase, new()
        //{
        //    return await Task.Run(()=> db.Deleteable<TSource>().Where(deleteObj).ExecuteCommand() > 0);
        //}

        /// <summary>
        /// 物理删除实体对象
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="db"></param>
        /// <param name="whereExp">条件表达式</param>
        /// <returns></returns>
        public static async Task<bool> Delete<TSource>(this SqlSugarClient db, Expression<Func<TSource, bool>> whereExp) where TSource : EntityBase, new()
        {
            return await Task.Run(() => db.Deleteable<TSource>().Where(whereExp).ExecuteCommand() > 0);
        }

        /// <summary>
        /// 根据主键物理删除实体对象
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="db"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        //public static async Task<bool> DeleteById<TSource>(this SqlSugarClient db, dynamic id) where TSource : EntityBase, new()
        //{
        //    return await Task.Run(() => db.Deleteable<TSource>().In(id).ExecuteCommand() > 0);
        //}

        /// <summary>
        /// 根据主键批量物理删除实体集合
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="db"></param>
        /// <param name="ids"></param>
        /// <returns></returns>
        //public static async Task<bool> DeleteByIds<TSource>(this SqlSugarClient db, dynamic[] ids) where TSource : EntityBase, new()
        //{
        //    return await Task.Run(()=> db.Deleteable<TSource>().In(ids).ExecuteCommand() > 0);
        //}

        #endregion

    }
}
