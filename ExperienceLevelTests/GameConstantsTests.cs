// Bernard Allotey 11-26-2019

using ExperienceLevel;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ExperienceLevelTests
{
    [TestClass]
    public class GameConstantsTests
    {
        [TestMethod]
        public void EnsureCallingWithoutInstanceThrows()
        {
            Assert.ThrowsException<GameConstants.NotInstantiatedException>(() => GameConstants.Queues);
        }

        [TestMethod]
        public void EnsureCorrectRetrievalOfSeasons()
        {
            GameConstants.LookupLists();
            var seasons = GameConstants.Seasons;
            foreach (var seasonDt in seasons)
            {
                if (seasonDt.Season == "PRESEASON 3")
                {
                    return;
                }
            }
            Assert.Fail("Did not find Preseason 3...");
        }
    }
}