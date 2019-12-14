// Bernard Allotey 11-29-2019

using System.Collections.Generic;
using System.Linq;
using ExperienceLevel;
using ExperienceLevel.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;

namespace ExperienceLevelTests
{
    [TestClass]
    public class IntegrationTests
    {
        [TestMethod]
        public void EnsureCorrectWebLookup()
        {
            var summoner = Summoner.FromJson(WebIo.GetSummonerString("pentacalc"));
            Assert.AreEqual("pentacalc", summoner.Name);
            Assert.AreEqual(13, summoner.SummonerLevel);
            var matches = JsonConvert.DeserializeObject<MatchList>(WebIo.GetMatchListString(summoner.AccountId));
            Assert.IsTrue(matches.Matches.Any(reference => reference.GameId == 3158645188));
            
        }

        [TestMethod]
        public void EnsureSpecificMatchHistory()
        {
            var summoner = Summoner.FromJson(WebIo.GetSummonerString("pentacalc"));
            var matches = JsonConvert.DeserializeObject<MatchList>(WebIo.GetMatchListString(summoner.AccountId,
                new List<Champion> {Champion.GetChampionByKey(3)},
                new List<int> {420, 400, 430, 440}));
            Assert.IsTrue(matches.Matches.Any(reference => reference.Champion == 3));
        }
    }
}