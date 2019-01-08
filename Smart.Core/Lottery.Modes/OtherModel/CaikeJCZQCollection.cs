using System;
using System.Collections.Generic;
using System.Text;

namespace Lottery.Modes.OtherModel
{
    public class CaikeCommonCollection
    {
        public int resultCode { get; set; }

        public Caike_Body body { get; set; }
    }

    public class Caike_Body
    {
        public string afterPage { get; set; }
        public bool hasNext { get; set; }
        public List<Caike_matchDates> matchDates { get; set; }
        public List<Caike_records> records { get; set; }
    }

    public class Caike_matchDates
    {
        public string code { get; set; }
        public string name { get; set; }
    }


    public class Caike_records
    {
        public List<Caike_DetailModel> details { get; set; }
        public string groupTitle { get; set; }
        public string guestTeam { get; set; }
        public string hScoreText { get; set; }
        public string homeTeam { get; set; }
        public string leagueName { get; set; }
        public string matchNo { get; set; }
        public string scheduleId { get; set; }
        public string scoreText { get; set; }
    }
    public class Caike_DetailModel
    {
        public string name { get; set; }
        public string value { get; set; }
    }
}
