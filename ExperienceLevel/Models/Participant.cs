// Bernard Allotey 11-26-2019

using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace ExperienceLevel.Models
{
    public struct Participant
    {
        /// <summary>
        /// Participant Statistics
        /// </summary>
        public ParticipantStats Stats { get; set; }
        public int ParticipantId { get; set; }
        /// <summary>
        /// List of legacy Rune information.<para></para>
        /// Not included for matches played with Runes Reforged.
        /// </summary>
        public List<Rune> Runes { get; set; }
        /// <summary>
        /// Participant timeline data
        /// </summary>
        public ParticipantTimeline Timeline { get; set; }
        private int _teamId;
        /// <summary>
        /// Second summoner spell id
        /// </summary>
        public int Spell2Id { get; set; }
        /// <summary>
        /// List of legacy mastery information.<para/>
        /// Not included for matches played with Runes Reforged
        /// </summary>
        public List<Mastery> Masteries { get; set; }
        private GameConstants.MasteryTier _highestAchievedSeasonTier;
        /// <summary>
        /// First summoner spell id
        /// </summary>
        public int Spell1Id { get; set; }
        public int ChampionId { get; set; }

        public int TeamId
        {
            get => _teamId;
            set
            {
                if (value != 100 && value != 200)
                    throw new InvalidOperationException("Team id: " + value + " must be either 100 or 200");
                _teamId = value;
            }
        }

        public string HighestAchievedSeasonTier
        {
            get => _highestAchievedSeasonTier.Value;
            set
            {
                try
                {
                    _highestAchievedSeasonTier = GameConstants.MasteryTier.BuildFromString(value);
                }
                catch (InvalidEnumArgumentException e)
                {
                    Console.WriteLine(e);
                    throw;
                }
            }
        }
        
    }
}