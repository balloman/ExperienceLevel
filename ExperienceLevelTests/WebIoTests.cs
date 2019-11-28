using System;
using System.Collections.Generic;
using System.Text;
using ExperienceLevel;
using ExperienceLevel.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ExperienceLevelTests
{
    [TestClass]
    public class WebIoTests
    {
        private string _summonerString;

        private static Summoner TestSummoner()
        {
            var summoner = new Summoner
            {
                AccountId = "Ix60OvqS_m1e8qOnw2uKfh25MqfdafuvOMmFSuvp5H8e940",
                Id = "gIjao8Gs5ux_XtLggnb2-KiAHbXIPctgwO2HI9LGmEZVaZI",
                Name = "pentacalc",
                ProfileIconId = 3554,
                PuuId = "BVNwjBasJEOX-8DPoWECI968V-fnrGUylLcVL6VqTuAvxk5-KNWo-H9c51brrtWhsu01muUYHPmsuQ",
                RevisionDate = 1569461540000,
                SummonerLevel = 13
            };
            return summoner;
        }

        [TestInitialize]
        public void GrabData()
        {
            _summonerString = WebIo.GetSummonerString("pentacalc");
        }

        [TestMethod]
        public void TestCorrectSummonerData()
        {
            Assert.AreEqual(TestSummoner().AccountId, Summoner.FromJson(_summonerString).AccountId);
        }
    }
}
