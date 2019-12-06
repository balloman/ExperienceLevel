// Bernard Allotey 11-25-2019

using System.Collections.Generic;
using Newtonsoft.Json;

namespace ExperienceLevel.Models
{
    public struct MatchList
    {
        public List<MatchReference> Matches { get; set; }
        public int TotalGames { get; set; }
        public int StartIndex { get; set; }
        public int EndIndex { get; set; }

        public static MatchList FromJson(string matchListString)
        {
            return JsonConvert.DeserializeObject<MatchList>(matchListString);
        }
    }
}