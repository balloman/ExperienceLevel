// Bernard Allotey 11-26-2019

using System;
using System.Collections.Generic;

namespace ExperienceLevel.Models
{
    public struct TeamStats
    {
        /// <summary>
        /// Flag indicating whether or not the team scored the first Dragon kill.
        /// </summary>
        public bool FirstDragon { get; set; }
        /// <summary>
        /// Flag indicating whether or not the team destroyed the first inhibitor
        /// </summary>
        public bool FirstInhibitor { get; set; }
        /// <summary>
        /// If match queueId has a draft, contains banned champion data, otherwise empty
        /// </summary>
        public List<TeamBans> Bans { get; set; }
        /// <summary>
        /// Number of times the team killed Baron.
        /// </summary>
        public int BaronKills { get; set; }
        /// <summary>
        /// Flag indicating whether or not the team scored the first Rift Herald kill
        /// </summary>
        public bool FirstRiftHerald { get; set; }
        /// <summary>
        /// Flag indicating whether or not the team scored the first Baron kill.
        /// </summary>
        public bool FirstBaron { get; set; }
        /// <summary>
        /// Number of times the team killed Rift Herald
        /// </summary>
        public int RiftHeraldKills { get; set; }
        /// <summary>
        /// Flag indicating whether or not the team scored the first blood.
        /// </summary>
        public bool FirstBlood { get; set; }
        private int _teamId;
        /// <summary>
        /// Flag indicating whether or not the team destroyed the first tower
        /// </summary>
        public bool FirstTower { get; set; }
        /// <summary>
        /// Number of times the team killed Vilemaw
        /// </summary>
        public int VilemawKills { get; set; }
        /// <summary>
        /// Number of inhibitors the team destroyed
        /// </summary>
        public int InhibitorKills { get; set; }
        /// <summary>
        /// Number of towers the team destroyed
        /// </summary>
        public int TowerKills { get; set; }
        /// <summary>
        /// For Dominion matches, specifies the points the team had at game end
        /// </summary>
        public int DominionVictoryScore { get; set; }
        private string _win;
        /// <summary>
        /// Number of times the team killed the Dragon.
        /// </summary>
        public int DragonKills { get; set; }
        
        /// <summary>
        /// 100 for blue side, 200 for red side
        /// </summary>
        /// <exception cref="InvalidOperationException"></exception>
        public int TeamId
        {
            get => _teamId;
            set
            {
                if (value != 100 && value != 200)
                {
                    throw new InvalidOperationException("Team Id: " + value + " can only be 100 or 200");
                }
                else
                {
                    _teamId = value;
                }
            }
        }

        /// <summary>
        /// String indicating whether or not the team won. There are only two values visible in public match history.<para></para>
        /// Legal values: Fail, Win<para></para>
        /// </summary>
        /// <exception cref="InvalidOperationException"></exception>
        public string Win
        {
            get => _win;
            set
            {
                if (value.ToLower() == "win" || value.ToLower() == "fail")
                {
                    _win = value;
                }
                else
                {
                    throw new InvalidOperationException("Value must be either Fail or Win");
                }
            }
        }
    }
}