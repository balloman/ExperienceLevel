// Bernard Allotey 11-25-2019

namespace ExperienceLevel.Models
{
    public struct TeamBans
    {
        /// <summary>
        /// Turn during which the champion was banned
        /// </summary>
        public int PickTurn { get; set; }
        /// <summary>
        /// Banned championId
        /// </summary>
        public int ChampionId { get; set; }
    }
}