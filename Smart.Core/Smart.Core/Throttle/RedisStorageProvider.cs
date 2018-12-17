using Smart.Core.NoSql.Redis;
using Smart.Core.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smart.Core.Throttle
{
    internal class RedisStorageProvider : IStorageProvider
    {
        protected readonly CSRedis.CSRedisClient _db;
        public RedisStorageProvider(RedisManager redisManager)
        {
            _db = redisManager.RedisDb(0);
        }

        /// <summary>
        /// 添加名单
        /// </summary>
        /// <param name="rosterType">名单类型</param>
        /// <param name="policy">策略</param>
        /// <param name="policyKey">策略Key</param>
        /// <param name="expiry">过期时间</param>
        /// <param name="item">项目</param>
        /// <remarks>因为要保存过期时间，所以名单通过Redis 有序集合(sorted set)来存储，score来存储过期时间Ticks</remarks>
        public async Task AddRosterAsync(RosterType rosterType, string api, Policy policy, string policyKey, TimeSpan? expiry, params string[] item)
        {
            if (item == null || item.Length == 0)
            {
                return;
            }

            //过期时间计算
            double score = expiry == null ? double.PositiveInfinity : DateTime.Now.Add(expiry.Value).Ticks;


            var key = FromatRosterKey(rosterType, api, policy, policyKey);
            //保存
            await _db.ZAddAsync(key, (score, item));

            //删除过期名单数据
            await _db.ZRemRangeByScoreAsync(key, 0, DateTime.Now.Ticks);
        }

        /// <summary>
        /// 删除名单中数据
        /// </summary>
        /// <param name="rosterType">名单类型</param>
        /// <param name="api">API</param>
        /// <param name="policy">策略</param>
        /// <param name="policyKey">策略Key</param>
        /// <param name="expiry">过期时间</param>
        /// <param name="item">项目</param>
        public async Task RemoveRosterAsync(RosterType rosterType, string api, Policy policy, string policyKey, params string[] item)
        {
            if (item == null || item.Length == 0)
            {
                return;
            }
            var key = FromatRosterKey(rosterType, api, policy, policyKey);
            //删除
            await _db.ZRemAsync(key, item);
        }



        /// <summary>
        /// 取得名单列表（分页）
        /// </summary>
        /// <param name="rosterType">名单类型</param>
        /// <param name="api">API</param>
        /// <param name="policy">策略</param>
        /// <param name="policyKey">策略Key</param>
        public async Task<(long count, IEnumerable<ListItem> items)> GetRosterListAsync(RosterType rosterType, string api, Policy policy, string policyKey, long skip, long take)
        {
            var key = FromatRosterKey(rosterType, api, policy, policyKey);
            //取得件数
            var count = await _db.ZCardAsync(key);

            if (count == 0)
            {
                return (0, new List<ListItem>());
            }

            //取得数据
            var data = await _db.ZRangeWithScoresAsync(key, 0, -1);

            return (count, data.Select(x => new ListItem { Value = x.member, ExpireTicks = x.score }));
        }

        /// <summary>
        /// 取得名单列表
        /// </summary>
        /// <param name="rosterType">名单类型</param>
        /// <param name="api">API</param>
        /// <param name="policy">策略</param>
        /// <param name="policyKey">策略Key</param>
        public async Task<IEnumerable<ListItem>> GetRosterListAsync(RosterType rosterType, string api, Policy policy, string policyKey)
        {
            var key = FromatRosterKey(rosterType, api, policy, policyKey);

            //取得数据
            var data = await _db.ZRangeWithScoresAsync(key, 0, -1);
            foreach (var item in data)
            {

            }
            return data.Select(x => new ListItem { Value = x.member, ExpireTicks = x.score });
        }



        private string FromatRosterKey(RosterType rosterType, string api, Policy policy, string policyKey)
        {
            var key = $"apithrottle::storage:{rosterType.ToString().ToLower()}:{policy.ToString().ToLower()}";
            if (!string.IsNullOrEmpty(policyKey))
            {
                key += ":" + MD5Util.EncryptMD5Short(policyKey);
            }
            key += ":" + api.ToLower();
            return key;
        }
    }
}
