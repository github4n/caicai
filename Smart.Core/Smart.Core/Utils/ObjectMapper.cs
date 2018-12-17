using System.Collections.Generic;

namespace Smart.Core.Utils
{
    /// <summary>
    /// 功能描述    ：配置装饰器，配置文件有的优先使用
    /// </summary>
    public static class ObjectMapper
    {
        #region Public Methods

        /// <summary>
        /// 对应mapper映射：如果源对象file的属性不为空，就赋值给obj对应的属性，否则obj属性不变
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj">要赋值的对象</param>
        /// <param name="file">源对象</param>
        public static void MapperTo<T>(T obj, T file)
        {
            if (file != null)
            {
                foreach (var item in typeof(T).GetProperties())
                {
                    if (item.GetValue(file) != null)
                        item.SetValue(obj, item.GetValue(file));
                }
            }
        }

        /// <summary>
        /// 将源数据转为目标类型对象
        /// </summary>
        /// <typeparam name="From"></typeparam>
        /// <typeparam name="To"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static To MapperTo<From, To>(this From obj) where To : new()
        {
            To des = new To();
            if (obj != null)
            {
                foreach (var item in typeof(To).GetProperties())
                {
                    var objP = obj.GetType().GetProperty(item.Name);
                    if (objP != null)
                    {
                        item.SetValue(des, objP.GetValue(obj));
                    }
                }
            }
            return des;
        }

        /// <summary>
        /// 将源集合转为目标集合
        /// </summary>
        /// <typeparam name="IEnumerable"></typeparam>
        /// <typeparam name="To"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static IEnumerable<To> MapperTo<From, To>(this IEnumerable<From> obj) where To : new()
        {
            List<To> list = new List<To>();
            foreach (var item in obj)
            {
                list.Add(MapperTo<From, To>(item));
            }
            return list;
        }

        #endregion Public Methods
    }
}
