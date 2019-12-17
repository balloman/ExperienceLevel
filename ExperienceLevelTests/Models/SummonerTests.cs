// Bernard Allotey 11-25-2019

using ExperienceLevel;
using ExperienceLevel.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ExperienceLevelTests.Models
{
    [TestClass]
    public class SummonerTests
    {
        private Summoner _summoner;

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
        public void CreateNewSummoner()
        {
            _summoner = Summoner.FromJson(GeneralIo.GetSummonerString("pentacalc"));
        }

        [TestMethod]
        public void CorrectConversionFromStringToJson()
        {
            var summoner = Summoner.FromJson("{\n" +
                                                  "\"profileIconId\": 3554,\n" +
                                                  "\"name\": \"pentacalc\",\n" +
                                                  "\"puuid\": \"BVNwjBasJEOX-8DPoWECI968V-fnrGUylLcVL6VqTuAvxk5-KNWo-H9c51brrtWhsu01muUYHPmsuQ\",\n" +
                                                  "\"summonerLevel\": 13,\n" +
                                                  "\"accountId\": \"Ix60OvqS_m1e8qOnw2uKfh25MqfdafuvOMmFSuvp5H8e940\",\n" +
                                                  "\"id\": \"gIjao8Gs5ux_XtLggnb2-KiAHbXIPctgwO2HI9LGmEZVaZI\",\n" +
                                                  "\"revisionDate\": 1569461540000\n" +
                                                  "}");
            StringAssert.Contains(TestSummoner().ToString(), summoner.ToString());
        }

        [TestMethod]
        public void CorrectGetMatchLists()
        {
            Assert.IsNotNull(_summoner.MatchList());
        }
    }
}