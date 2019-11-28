// Bernard Allotey 11-25-2019

using System;
using System.Collections.Generic;
using System.Net.Http;
using Newtonsoft.Json;

namespace ExperienceLevel.Models
{
    public class Summoner
    {
        public int ProfileIconId { get; set; }
        public string Name { get; set; }
        public string PuuId { get; set; }
        public long SummonerLevel { get; set; }
        public long RevisionDate { get; set; }
        public string Id { get; set; }
        public string AccountId { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }

        public static Summoner FromJson(string jsonString)
        {
            return JsonConvert.DeserializeObject<Summoner>(jsonString);
        }

        public List<Match> MatchList(HashSet<Champion> champions = null, HashSet<int> queue = null, long endTime = 0,
            long beginTime = 0, int endIndex = 0, int beginIndex = 0)
        {
            var list = JsonConvert.DeserializeObject<MatchList>(WebIo.GetMatchListString(AccountId, champions, queue,
                endTime, beginTime, endIndex, beginIndex));
            return list.Matches;
        }
    }
}