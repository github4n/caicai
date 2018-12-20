using System;
using System.Collections.Generic;
using System.Text;

namespace Smart.Core.NoSql.Redis
{
   public class RedisConfig
    {
        /// <summary>
        /// 连接ip
        /// </summary>
        public string C_IP { get; set; } = "10.0.3.6";
        /// <summary>
        /// 连接端口
        /// </summary>
        public int C_Post { get; set; } = 6379;
        /// <summary>
        /// 密码
        /// </summary>
        public string C_Password { get; set; } = "redis123";
        /// <summary>
        /// 默认数据库
        /// </summary>
        public int C_Defaultdatabase { get; set; } = 0;
        /// <summary>
        /// 连接池大小
        /// </summary>
        public int C_PoolSize { get; set; } = 50;
        /// <summary>
        /// 异步方法写入缓冲区大小(字节)
        /// </summary>
        public int c_Writebuffer { get; set; } = 10240;
        /// <summary>
        ///  key前辍，所有方法都会附带此前辍，csredis.Set(prefix + "key", 111);
        /// </summary>
        public string C_Prefix { get; set; } = "";
        /// <summary>
        /// 是否开启加密传输
        /// </summary>
        public bool C_SSL { get; set; } = false;
        /// <summary>
        /// 预热连接
        /// </summary>
        public bool C_Preheat { get; set; } = true;
    }
}
