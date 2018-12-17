
namespace Smart.Core.NoSql.Mongodb
{
    /// <summary>
    /// mongodb配置
    ///  "MongoConfig": {
    ///  "Host": "192.168.200.214:27017",
    ///  "AuthUser": "",
    ///  "Password": "",
    ///  "DbName": "test"}
    /// </summary>
    public class MongodbConfig
    {
        /// <summary>
        /// 服务器地址
        /// </summary>
        public string Host { get; set; } = "localhost://27017";

        /// <summary>
        /// 授权用户
        /// </summary>
        public string AuthUser { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// 主数据库
        /// </summary>
        public string DbName { get; set; }
    }
}
