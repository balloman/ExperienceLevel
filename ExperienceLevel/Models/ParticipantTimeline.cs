// Bernard Allotey 11-26-2019

using System.Collections.Generic;

namespace ExperienceLevel.Models
{
    public struct ParticipantTimeline
    {
        /// <summary>
        /// Participant's calculated lane. MID and Bot are legacy values<para></para>
        /// Legal Values: MID, MIDDLE, TOP, JUNGLE, BOT, BOTTOM
        /// </summary>
        public string Lane { get; set; }
        public int ParticipantId { get; set; }
        /// <summary>
        /// Creep score difference versus the calculated lane opponent(s) for a specified period
        /// </summary>
        public Dictionary<string, double> CsDiffPerMinDeltas { get; set; }
        /// <summary>
        /// Gold for a specified period
        /// </summary>
        public Dictionary<string, double> GoldPerMinDeltas { get; set; }
        /// <summary>
        /// Experience difference versus the calculated lane opponent(s) for a specified period.
        /// </summary>
        public Dictionary<string, double> XpDiffPerMinDeltas { get; set; }
        /// <summary>
        /// Creeps for a specified period.
        /// </summary>
        public Dictionary<string, double> CreepsPerMinDeltas { get; set; }
        /// <summary>
        /// Experience change for a specified period.
        /// </summary>
        public Dictionary<string, double> XpPerMinDeltas { get; set; }
        /// <summary>
        /// Participant's calculated role.<para></para>
        /// Legal Values: DUO, NONE, SOLO, DUO_CARRY, DUO_SUPPORT
        /// </summary>
        public string Role { get; set; }
        /// <summary>
        /// Damage taken difference versus the calculated lane opponent(s) for a specified period.
        /// </summary>
        public Dictionary<string, double> DamageTakenDiffPerMinDeltas { get; set; }
        /// <summary>
        /// Damage taken for a specified period.
        /// </summary>
        public Dictionary<string, double> DamageTakenPerMinDeltas { get; set; }
    }
}