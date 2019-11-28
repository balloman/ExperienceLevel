// Bernard Allotey 11-25-2019

using System;
using System.Collections.Generic;
using System.Linq;

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
        /// <summary>
        /// Team information
        /// </summary>
        public List<TeamStats> Teams { get; set; }
        /// <summary>
        /// Participant Information
        /// </summary>
        public List<Participant> Participants { get; set; }
        /// <summary>
        /// Match duration in seconds
        /// </summary>
        public long GameDuration { get; set; }
        /// <summary>
        /// Designates the timestamp when champion select ended and the loading screen appeared,<para></para>
        /// NOT when the game timer was at 0:00.
        /// </summary>
        public long GameCreation { get; set; }
        
        public int SeasonId
        {
            get => _seasonId;
            set
            {
                if (GameConstants.Seasons.All(seasonDt => seasonDt.Id != value))
                    throw new GameConstants.ConstantNotFoundException(value.ToString());
                _seasonId = value;
            }
        }

        public int QueueId
        {
            get => _queueId;
            set
            {
                if (GameConstants.Queues.All(queueDt => queueDt.QueueId != value))
                    throw new GameConstants.ConstantNotFoundException(value.ToString());
                _queueId = value;
            }
        }

        public string GameMode
        {
            get => _gameMode;
            set
            {
                if (GameConstants.GameModes.All(dt => dt.GameMode != value))
                    throw new GameConstants.ConstantNotFoundException(value);
                _gameMode = value;
            }
        }

        public int MapId
        {
            get => _mapId;
            set
            {
                if (GameConstants.Maps.All(dt => dt.MapId != value))
                    throw new GameConstants.ConstantNotFoundException(value.ToString());
                _mapId = value;
            }
        }

        public string GameType
        {
            get => _gameType;
            set
            {
                if (GameConstants.GameTypes.All(dt => !string.Equals(dt.GameType, value, 
                    StringComparison.CurrentCultureIgnoreCase)))
                    throw new GameConstants.ConstantNotFoundException(value);
                _gameType = value;
            }
        }
    }
}