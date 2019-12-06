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
    }
}