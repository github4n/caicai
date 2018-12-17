using CSRedis;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Smart.Core.NoSql.Redis
{
    public class RedisManager
    {
        private List<RedisConfig> _configs;
        private static CSRedisClient[] redisClients = null;
        private static System.Collections.Hashtable RedisHas = System.Collections.Hashtable.Synchronized(new Hashtable());
        public RedisManager(List<RedisConfig> configs)
        {
            _configs = configs;
            Initialization();
        }

        public void Initialization()
        {
            redisClients = new CSRedisClient[this._configs.Count]; //定义成单例
            int i = 0;
            foreach (var item in this._configs)
            {
                //string key = $"{item.C_IP}:{item.C_Post}/{item.C_Defaultdatabase}";
                string key = $"{i}";
                var connectionString = $"{item.C_IP}:{item.C_Post},password={item.C_Password},defaultDatabase={item.C_Defaultdatabase},poolsize={item.C_PoolSize}," +
                    $"preheat=true,ssl=false,writeBuffer=10240,prefix={item.C_Prefix}";
                RedisHas[key] = new CSRedis.CSRedisClient(connectionString);
                redisClients[i] = new CSRedis.CSRedisClient(connectionString);
                i++;
            }
        }


        public CSRedis.CSRedisClient RedisDb(int db)
        {
            var redis = redisClients[db] as CSRedis.CSRedisClient;
            return redis;
        }

        public CSRedis.CSRedisClient RedisDb(string key)
        {
            var redis = RedisHas[key] as CSRedis.CSRedisClient;
            return redis;
        }

    }
}
