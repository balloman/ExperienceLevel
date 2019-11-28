// Bernard Allotey 11-25-2019

namespace ExperienceLevel.Models
{
    public struct Player
    {
        public string CurrentPlatformId { get; set; }
        public string SummonerName { get; set; }
        public string MatchHistoryUri { get; set; }
        /// <summary>
        /// Original platformId
        /// </summary>
        public string PlatformId { get; set; }
        /// <summary>
        /// Player's current accountId (Encrypted)
        /// </summary>
        public string CurrentAccountId { get; set; }
        public int ProfileIcon { get; set; }
        /// <summary>
        /// Player's summonerId (Encrypted)
        /// </summary>
        public string SummonerId { get; set; }
        /// <summary>
        /// Player's original accountId (Encrypted)
        /// </summary>
        public string AccountId { get; set; }
    }
}