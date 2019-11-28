// Bernard Allotey 11-26-2019

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
        public ParticipantTimeline Timeline { get; set; }
        public int TeamId { get; set; }
        public int Spell2Id { get; set; }
        public List<Mastery> Masteries { get; set; }
        public string HighestAchievedSeasonTier { get; set; }
        public int Spell1Id { get; set; }
        public int ChampionId { get; set; }
    }
}