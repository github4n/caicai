using System;
using System.Collections.Generic;
using System.Text;

namespace Lottery.Modes.Model
{
   public class HeightLottery
    {
        public string key { get; set; }
        public string title { get; set; }
        public string title_url { get; set; }
        public foot_group foot_group { get; set; }
    }
    public class foot_group
    {
        public string url { get; set; }
        public string remark { get; set; }
    }
}
