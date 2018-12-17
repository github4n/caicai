using Lottery.GatherApp.Analysis.HK6;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lottery.GatherApp.Analysis
{
    public static class AnalysisManager
    {
        private static System.Collections.Hashtable _dicLcItemUpd = System.Collections.Hashtable.Synchronized(new System.Collections.Hashtable());
        /// <summary>
        /// CQSSC开奖期号|开奖号
        /// </summary>
        /// <param name="type"></param>
        public static List<string> CQSSC(string type)
        {
            List<string> list = new List<string>();
            switch (type)
            {
                case "129kai":
                    list = CQSSC_129kai.Analysis();
                    break;
                case "168":
                    list = CQSSC_168.Analysis();
                    break;
                case "opencai":
                    list = CQSSC_opencai.Analysis();
                    break;
                case "16cp":
                    list = CQSSC_16cp.Analysis();
                    break;
                case "cle":
                    list = CQSSC_cle.Analysis();
                    break;
            }
            if (list.Count > 0)
            {
                var data = CheckUpdData($"CQSSC_{type}", list);
                if (data.Count > 0)
                {
                    return data;
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// HK6
        /// </summary>
        /// <returns></returns>
        public static List<string> HK6()
        {
            List<string> list = HK6_168.Analysis();
            if (list.Count > 0)
            {
                var data = CheckUpdData("HK6_168", list);
                if (data.Count > 0)
                {
                    return data;
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }

        public static List<string> HK6Issue()
        {
            List<string> list = HK6_168.AnalysisIssue();
            if (list.Count > 0)
            {
                var data = CheckUpdData("HK6_168_Issue", list);
                if (data.Count > 0)
                {
                    return data;
                }
                else
                {
                    return null;
                }
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
