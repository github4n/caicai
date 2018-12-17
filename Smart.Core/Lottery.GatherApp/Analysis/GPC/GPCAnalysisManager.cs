using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lottery.GatherApp
{
   public class GPCAnalysisManager
    {
        private static System.Collections.Hashtable _dicLcItemUpd = System.Collections.Hashtable.Synchronized(new System.Collections.Hashtable());

        /// <summary>
        /// HN5FZ开奖期号|开奖号
        /// </summary>
        /// <returns></returns>
        public static List<string> HN5FZ(string issue)
        {
            if (!_dicLcItemUpd.ContainsKey($"HN5FZ_{issue}"))
            {
                string code = HN5FZ_my.Analysis();
                _dicLcItemUpd[$"HN5FZ_{issue}"] = $"{issue}|{code}";
                return new List<string>() { $"{issue}|{code}" };
            }
            else
            {
                return null;
            }
        }


        /// <summary>
        /// HN2FZ|开奖号
        /// </summary>
        /// <returns></returns>
        public static List<string> HN2FZ(string issue)
        {
            if (!_dicLcItemUpd.ContainsKey($"HN2FZ_{issue}"))
            {
                string code = HN2FZ_my.Analysis();
                _dicLcItemUpd[$"HN2FZ_{issue}"] = $"{issue}|{code}";
                return new List<string>() { $"{issue}|{code}" };
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// HN1FZ|开奖号
        /// </summary>
        /// <returns></returns>
        public static List<string> HN1FZ(string issue)
        {
            if (!_dicLcItemUpd.ContainsKey($"HN1FZ_{issue}"))
            {
                string code = HN1FZ_my.Analysis();
                _dicLcItemUpd[$"HN1FZ_{issue}"] = $"{issue}|{code}";
                return new List<string>() { $"{issue}|{code}" };
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// BXKLC|开奖号
        /// </summary>
        /// <returns></returns>
        public static List<string> BXKLC(string issue)
        {
            if (!_dicLcItemUpd.ContainsKey($"BXKLC_{issue}"))
            {
                string code = HN1FZ_my.Analysis();
                _dicLcItemUpd[$"BXKLC_{issue}"] = $"{issue}|{code}";
                return new List<string>() { $"{issue}|{code}" };
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// BX15FC|开奖号
        /// </summary>
        /// <returns></returns>
        public static List<string> BX15FC(string issue)
        {
            if (!_dicLcItemUpd.ContainsKey($"BX15FC_{issue}"))
            {
                string code = BX15FC_my.Analysis();
                _dicLcItemUpd[$"BX15FC_{issue}"] = $"{issue}|{code}";
                return new List<string>() { $"{issue}|{code}" };
            }
            else
            {
                return null;
            }
        }

        private static List<string> CheckUpdData(string key, List<string> lsInited)
        {
            List<string> list = new List<string>();
            if (_dicLcItemUpd.ContainsKey(key))
            {
                list = lsInited.Except((List<string>)_dicLcItemUpd[key]).ToList();
                if (list.Count > 0)
                    _dicLcItemUpd[key] = lsInited;
            }
            else
            {
                list = lsInited;
                _dicLcItemUpd[key] = lsInited;
            }
            return list;
        }

        public static void DelUpdData(string key)
        {
            if (_dicLcItemUpd.ContainsKey(key))
            {
                _dicLcItemUpd.Remove(key);
            }
        }
    }
}
