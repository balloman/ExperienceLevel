// Bernard Allotey 11-25-2019

using System.Collections.Generic;

namespace ExperienceLevel.Models
{
    public struct MatchList
    {
        public List<Match> Matches { get; set; }
        public int TotalGames { get; set; }
        public int StartIndex { get; set; }
        public int EndIndex { get; set; }
    }
}