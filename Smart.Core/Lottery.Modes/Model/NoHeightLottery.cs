using System;
using System.Collections.Generic;
using System.Text;

namespace Lottery.Modes.Model
{
   public class NoHeightLottery
    {
        public string key { get; set; }
        public string title { get; set; }
        public string title_url { get; set; }
        public string morelink { get; set; }
        public List<foot_group> foot_group { get; set; }
    }
}
