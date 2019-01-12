using System;

using Smart.Core.Repository;

using SqlSugar;

namespace Lottery.Modes.Model
{
    public class IP:EntityBase
    {
        /// <summary>
        /// 主键
        /// </summary>
        [SugarColumn(IsNullable = false, IsPrimaryKey = true, IsIdentity = true)]
        public int ID { get; set; }
        /// <summary>
        /// IP地址
        /// </summary>
        public string IPAddress { get; set; }
        /// <summary>
        /// 端口
        /// </summary>
        public string Port { get; set; }
        /// <summary>
        /// 类型（http/https）
        /// </summary>
        public string Type { get; set; }
        /// <summary>
        /// 速度
        /// </summary>
        public string Speed { get; set; }
        /// <summary>
        /// 连接时间
        /// </summary>
        public string ConnectionTime { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 失败次数
        /// </summary>
        public int FailNum { get; set; }
    }
}
