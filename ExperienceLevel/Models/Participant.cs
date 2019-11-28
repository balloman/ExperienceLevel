// Bernard Allotey 11-26-2019

using System;
using System.Collections.Generic;

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
        private string _highestAchievedSeasonTier;
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
                if (_teamId != 100 && _teamId != 200)
                    throw new InvalidOperationException("Team id must be either 100 or 200");
                _teamId = value;
            }
        }

        public string HighestAchievedSeasonTier
        {
            get
            {
                
            }
            set { throw new NotImplementedException(); }
        }
    }
}