using System;
using MongoDB.Driver;

namespace Smart.Core.NoSql.Mongodb
{
    /// <summary>
    /// Mongodb驱动,数据库为当前用户授权的库，如果操作多库，应该有多个配置节点
    /// DI注入时使用单例
    /// </summary>
    public class MongodbManager
    {
        #region Private Fields

        private MongodbConfig _config;

        #endregion Private Fields

        #region Public Constructors

        public MongodbManager(MongodbConfig config)
        {
            _config = config;
        }

        #endregion Public Constructors

        #region MongoDB配置

        private string ConnectionString()
        {
            var database = _config.DbName;
            var userName = _config.AuthUser;
            var password = _config.Password;
            var authentication = string.Empty;
            var host = string.Empty;
            if (!string.IsNullOrWhiteSpace(userName))
            {
                authentication = string.Concat(userName, ':', password, '@');
            }
            database = database ?? "Test";
            if (string.IsNullOrWhiteSpace(_config.Host))
            {
                throw new ArgumentNullException("请配置MongoDB_Host节点");
            }
            return string.Format("mongodb://{0}{1}/{2}", authentication, _config.Host, database);
        }

        #endregion MongoDB配置

        #region Private Fields

        private static IMongoDatabase _instance;
        private static object lockObj = new object();

        #endregion Private Fields

        #region Public Properties

        /// <summary>
        /// MongoDB使用者，数据库由配置文件决定
        /// </summary>
        private IMongoDatabase Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (lockObj)
                    {
                        if (_instance == null)
                        {
                            var svrSettings = MongoUrl.Create(ConnectionString());
                            var server = new MongoClient(svrSettings);
                            _instance = server.GetDatabase(_config.DbName);
                        }
                    }
                }
                return _instance;
            }
        }

        /// <summary>
        /// 得到数据库
        /// </summary>
        /// <param name="dbName"></param>
        /// <returns></returns>
        public IMongoDatabase GetDb(string dbName)
        {
            var svrSettings = MongoUrl.Create(ConnectionString());
            var server = new MongoClient(svrSettings);
            return server.GetDatabase(_config.DbName);
        }

        /// <summary>
        /// 得到集合－使用配置中的数据库
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public IMongoCollection<T> GetCollection<T>()
        {
            return Instance.GetCollection<T>(typeof(T).Name);
        }

        /// <summary>
        /// 得到集合－使用配置中的数据库
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name"></param>
        /// <returns></returns>
        public IMongoCollection<T> GetCollection<T>(string name)
        {
            return Instance.GetCollection<T>(name);
        }

        #endregion Public Properties
    }
}
