// Bernard Allotey 11-26-2019

using System.Linq;
using ExperienceLevel;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ExperienceLevelTests
{
    [TestClass]
    public class GameConstantsTests
    {
        [TestMethod]
        public void EnsureCorrectRetrievalOfSeasons()
        {
            var seasons = GameConstants.Seasons;
            if (seasons.Any(seasonDt => seasonDt.Season == "PRESEASON 3"))
            {
                return;
            }
            Assert.Fail("Did not find Preseason 3...");
        }

        [TestMethod]
        public void EnsureCorrectVersion()
        {
            var version = GameConstants.GameVersion;
            Assert.AreEqual(version, "lolpatch_3.7");
        }
    }
}