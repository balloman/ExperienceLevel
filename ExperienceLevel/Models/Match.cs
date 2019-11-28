// Bernard Allotey 11-25-2019

using System.Collections.Generic;

namespace ExperienceLevel.Models
{
    public class Match
    {
        private int _seasonId;
        private int _queueId;
        public long GameId { get; set; }
        /// <summary>
        /// Participant identity information
        /// </summary>
        public List<ParticipantIdentity> ParticipantIdentities { get; set; }
        /// <summary>
        /// The major.minor version typically indicates the patch the match was played on
        /// </summary>
        public string GameVersion { get; set; }
        /// <summary>
        /// The platform where the match was played
        /// </summary>
        public string PlatformId { get; set; }
        private string _gameMode;
        private int _mapId;
        private string _gameType;
        public List<TeamStats> Teams { get; set; }
        
    }
}