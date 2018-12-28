using System;
using System.Collections.Generic;
using System.Text;

namespace Lottery.Modes.Model
{
   public class JingCai
    {
        public string key { get; set; }
        public string url { get; set; }
        public List<links> links { get; set; }
    }
    public class links
    {
        public string key { get; set; }
        public string value { get; set; }
    }
}
