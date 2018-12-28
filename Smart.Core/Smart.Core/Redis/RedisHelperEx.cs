using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
//using StackExchange.Redis;
using System.Net;
using System.IO;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections;
using Smart.Core.Extensions;

namespace Smart.Core.Redis
{

    /// <summary>
    /// Redis数据库 CsRedisCode.RedisHelperEx
    /// </summary>
    public static class RedisHelperEx
    {

        static JObject RdConfigInfo = null;
        private static readonly object redisLock = new object();
        private static System.Collections.Hashtable RedisHas = System.Collections.Hashtable.Synchronized(new Hashtable());





        public static byte[] Serialize(object o)
        {
            if (o == null)
            {
                return null;
            }

            BinaryFormatter binaryFormatter = new BinaryFormatter();
            using (MemoryStream memoryStream = new MemoryStream())
            {
                binaryFormatter.Serialize(memoryStream, o);
                byte[] objectDataAsStream = memoryStream.ToArray();
                return objectDataAsStream;
            }
        }

        public static T Deserialize<T>(byte[] stream)
        {
            if (stream == null)
            {
                return default(T);
            }

            try
            {
                BinaryFormatter binaryFormatter = new BinaryFormatter();
                using (MemoryStream memoryStream = new MemoryStream(stream))
                {
                    T result = (T)binaryFormatter.Deserialize(memoryStream);
                    return result;
                }
            }
            catch
            {
                return default(T);
            }
        }

        public static List<T> Deserializes<T>(byte[] stream)
        {
            try
            {
                BinaryFormatter binaryFormatter = new BinaryFormatter();
                using (MemoryStream memoryStream = new MemoryStream(stream))
                {
                    var result = binaryFormatter.Deserialize(memoryStream) as List<T>;
                    return result;
                }
            }
            catch
            {
                return null;
            }
        }
    }
}
