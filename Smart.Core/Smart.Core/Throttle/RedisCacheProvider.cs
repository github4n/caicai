using Microsoft.Extensions.Options;
using Smart.Core.NoSql.Redis;
using Smart.Core.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smart.Core.Throttle
{
    internal class RedisCacheProvider : ICacheProvider
    {
        //缓存过期时间
        private TimeSpan? expiry = TimeSpan.FromHours(24);
        protected readonly CSRedis.CSRedisClient _db;
        private readonly IStorageProvider _storage;

        public RedisCacheProvider(RedisManager redisManager, IStorageProvider storage)
        {
            _db = redisManager.RedisDb(0);
            _storage = storage;
        }

        /// <summary>
        /// 取得计时时间段api调用次数
        /// </summary>
        public async Task<long> GetApiRecordCountAsync(string api, Policy policy, string policyKey, string policyValue, DateTime now, int duration)
        {
            var key = FromatApiRecordKey(api, policy, policyKey, policyValue);
            return await _db.ZCountAsync(key, now.Ticks - TimeSpan.FromSeconds(duration).Ticks, now.Ticks);
        }

        /// <summary>
        /// 保存调用记录
        /// </summary>
        public async Task AddApiRecordAsync(string api, Policy policy, string policyKey, string policyValue, DateTime now, int duration)
        {
            var key = FromatApiRecordKey(api, policy, policyKey, policyValue);

            await _db.ZAddAsync(key, (now.Ticks, now.Ticks.ToString()));

            //设置过期时间
            await _db.ExpireAsync(key, TimeSpan.FromSeconds(duration));
        }

        /// <summary>
        /// 取得名单列表
        /// </summary>
        public async Task<IEnumerable<ListItem>> GetRosterListAsync(RosterType rosterType, string api, Policy policy, string policyKey)
        {
            var key = FromatRosterKey(rosterType, api, policy, policyKey);
            //判断是否存在key
            if (await _db.ExistsAsync(key))
            {
                //取得数据
                var data = await _db.ZRangeWithScoresAsync(key, 0, -1);
                return data.Select(x => new ListItem { Value = x.member, ExpireTicks = x.score });
            }
            else
            {
                var data = (await _storage.GetRosterListAsync(rosterType, api, policy, policyKey)).ToList();

                //Ip地址转换
                if (policy == Policy.Ip)
                {
                    foreach (var item in data)
                    {
                        item.Value = CommonHelper.IpToNum(item.Value);
                        //保存
                        await _db.ZAddAsync(key, (item.ExpireTicks, item.Value));

                    }
                }
                //设置缓存过期时间
                await _db.ExpireAsync(key, expiry.Value);

                return data;
            }
        }


        /// <summary>
        /// 清除名单缓存
        /// </summary>
        /// <returns></returns>
        public async Task ClearRosterListCacheAsync(RosterType rosterType, string api, Policy policy, string policyKey)
        {
            var key = FromatRosterKey(rosterType, api, policy, policyKey);
            await _db.DelAsync(key);
        }


        private string FromatRosterKey(RosterType rosterType, string api, Policy policy, string policyKey)
        {
            var key = $"apithrottle:cache:{rosterType.ToString().ToLower()}:{policy.ToString().ToLower()}";
            if (!string.IsNullOrEmpty(policyKey))
            {
                key += ":" + MD5Util.EncryptMD5Short(policyKey);
            }
            key += ":" + api.ToLower();
            return key;
        }


        private string FromatApiRecordKey(string api, Policy policy, string policyKey, string policyValue)
        {
            var key = $"apithrottle:cache:record:{policy.ToString().ToLower()}";
            if (!string.IsNullOrEmpty(policyKey))
            {
                key += ":" + MD5Util.EncryptMD5Short(policyKey);
            }
            if (!string.IsNullOrEmpty(policyValue))
            {
                key += ":" + MD5Util.EncryptMD5Short(policyValue);
            }
            key += ":" + api.ToLower();
            return key;
        }
    }
}
