// Bernard Allotey 12-11-2019

using System;
using System.Linq;
using ExperienceLevel.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ExperienceLevelTests.Models
{
    [TestClass]
    public class ChampionTests
    {
        [TestMethod]
        public void EnsureChampionListWorksLol()
        {
            var champs = Champion.Champions;
            Assert.AreEqual(520, champs["Senna"].Stats.Hp);
        }

        [TestMethod]
        public void EnsureNoChampsHaveCritPerLevel()
        {
            var champs = Champion.Champions;
            Assert.IsTrue(!(champs.Any(pair => Math.Abs(pair.Value.Stats.CritPerLevel) > 0)));
        }
    }
}