using System;
using System.Collections.Generic;
using System.Text;

namespace Lottery.Modes.Model
{
    public class CommonModel
    {
        public string key { get; set; }
        public string Title { get; set; }
        public List<Item> Item { get; set; }
    }
    public class Item
    {
        public string Lottery { get; set; }
        public string DetailLinkUrl { get; set; }
        public string RemarkLinkUrl { get; set; }
    }
}
