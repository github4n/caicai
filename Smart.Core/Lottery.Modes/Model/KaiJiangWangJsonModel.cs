using System;
using System.Collections.Generic;
using System.Text;

namespace Lottery.Modes
{
    public class JsonReuslt
    {
        /// <summary>
        /// 
        /// </summary>
        public int code { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string message { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<KaiJiangWangJsonModel> content { get; set; }
    }
    public class KaiJiangWangJsonModel
    {
        /// <summary>
        /// 
        /// </summary>
        public List<int> preDrawCode { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string preDrawTime { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string preDrawIssue { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string sum { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string sumSingleDouble { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string sumBigSamll { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<string> bigSamlls { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<string> singleDoubles { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string firstThree { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string middleThree { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string lastThree { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string dragonTiger { get; set; }
    }
  
}
