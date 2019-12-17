// Bernard Allotey 11-29-2019

using System;
using System.Linq;
using ExperienceLevel;
using ExperienceLevel.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ExperienceLevelTests.Models
{
    [TestClass]
    public class MatchTests
    {
        private Match _match;
        
        [TestInitialize]
        public void AssignMatch()
        {
            var matchList = MatchList.FromJson(GeneralIo.GetMatchListString(Summoner.FromJson(
                GeneralIo.GetSummonerString("pentacalc")).AccountId));
            var matchReference = matchList.Matches.Find(reference => reference.GameId == 3158645188);
            _match = Match.FromJson(GeneralIo.GetMatchString(matchReference.GameId));
        }
        
        [TestMethod]
        public void EnsureCorrectMatchCreation()
        {
            Assert.IsTrue(GameConstants.GameModes.Any(dt => dt.GameMode.ToLower().Equals("classic")));
        }

        [TestMethod]
        public void EnsureCorrectParticipantIdentity()
        {
            Assert.IsTrue(_match.ParticipantIdentities.Any(identity => 
                identity.Player.SummonerName.Equals("pentacalc")));
            Assert.IsTrue(_match.ParticipantIdentities.Any(identity => identity.ParticipantId == 2));
        }
        
        [TestMethod]
        public void EnsureCorrectPlayer(){
            Assert.IsTrue(_match.ParticipantIdentities.Any(identity => 
                identity.Player.SummonerName.Equals("pentacalc")));
        }

        [TestMethod]
        public void EnsureCorrectTeamStats()
        {
            Assert.IsTrue(_match.Teams.Any(stats => stats.BaronKills == 1));
            Assert.IsTrue(_match.Teams.Any(stats => stats.Bans.Any(bans => bans.ChampionId == 64)));
        }

        [TestMethod]
        public void EnsureCorrectParticipant()
        {
            var participants = _match.Participants;
            Assert.IsTrue(participants.Any(participant => participant.ChampionId == 3));
            Assert.IsTrue(participants.Any(participant => 
                Math.Abs(participant.Timeline.GoldPerMinDeltas["20-30"] - 350.5) < 0.01));
            Assert.IsTrue(participants.Any(participant => participant.Stats.Kills == 3));
        }
        
        
    }
}