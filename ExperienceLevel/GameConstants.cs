// Bernard Allotey 11-26-2019

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;

namespace ExperienceLevel
{
    public static class GameConstants
    {
        private static bool _instanced = false;
        private static List<SeasonDt> _seasons;
        private static List<QueueDt> _queues;
        private static List<MapDt> _maps;
        private static List<GameModeDt> _gameModes;
        private static List<GameTypeDt> _gameTypes;
        private static string _gameVersion;

        /// <summary>
        /// Should attempt to lookup the jsons for all of the types<para></para>
        /// If not called, attempts to lookup any of the values will result in a <exception cref="NotInstantiatedException"></exception>
        /// </summary>
        private static void LookupLists()
        {
            var client = new HttpClient {BaseAddress = new Uri("http://static.developer.riotgames.com/docs/lol/")};
            Queues = JsonConvert.DeserializeObject<List<QueueDt>>(client.GetAsync("queues.json").Result.Content.ReadAsStringAsync().Result);
            Seasons = JsonConvert.DeserializeObject<List<SeasonDt>>(client.GetAsync("seasons.json").Result.Content.ReadAsStringAsync().Result);
            Maps = JsonConvert.DeserializeObject<List<MapDt>>(client.GetAsync("maps.json").Result.Content.ReadAsStringAsync().Result);
            GameModes = JsonConvert.DeserializeObject<List<GameModeDt>>(client.GetAsync("gameModes.json").Result.Content.ReadAsStringAsync().Result);
            GameTypes = JsonConvert.DeserializeObject<List<GameTypeDt>>(client.GetAsync("gameTypes.json").Result.Content.ReadAsStringAsync().Result);
            _instanced = true;
            client.Dispose();
        }

        public static List<SeasonDt> Seasons
        {
            get {
                if (_instanced)
                {
                    return _seasons;
                }
                else
                {
                    LookupLists();
                    return _seasons;
                }
            }
            private set => _seasons = value;
        }
        public static List<QueueDt> Queues
        {
            get
            {
                if (_instanced)
                {
                    return _queues;
                }
                else
                {
                    LookupLists();
                    return _queues;
                }
            }
            private set => _queues = value;
        }
        public static List<MapDt> Maps
        {
            get
            {
                if (_instanced)
                {
                    return _maps;
                }

                LookupLists();
                return _maps;
            }
            private set => _maps = value;
        }
        public static List<GameModeDt> GameModes
        {
            get
            {
                if (_instanced)
                {
                    return _gameModes;
                }

                LookupLists();
                return _gameModes;
            }
            private set => _gameModes = value;
        }
        public static List<GameTypeDt> GameTypes
        {
            get
            {
                if (_instanced)
                {
                    return _gameTypes;
                }
                LookupLists();
                return _gameTypes;
            }
            private set => _gameTypes = value;
        }

        /// <summary>
        /// Returns the current game version
        /// </summary>
        public static string GameVersion
        {
            get
            {
                if (!string.IsNullOrEmpty(_gameVersion)) return _gameVersion;
                
                var versions = JsonConvert.DeserializeObject<List<string>>(WebIo.GetVersionsString());
                _gameVersion = versions[0];
                return _gameVersion;
            }
        }

        /// <summary>
        /// Represents essentially just an enum for the mastery tiers
        /// </summary>
        public class MasteryTier
        {
            private MasteryTier(string value)
            {
                Value = value;
            }
            
            public string Value { get; }

            public static MasteryTier BuildFromString(string val)
            {
                var valid = new[]
                {
                    "Unranked",
                    "Bronze",
                    "Silver",
                    "Gold",
                    "Platinum",
                    "Diamond",
                    "Master",
                    "Challenger"
                };
                if (valid.All(s => !string.Equals(s, val, StringComparison.CurrentCultureIgnoreCase)))
                {
                    throw new InvalidEnumArgumentException(val + " tier does not exist...");
                }

                return new MasteryTier(val);
            }
            
            public static MasteryTier Unranked => new MasteryTier("Unranked");
            public static MasteryTier Bronze => new MasteryTier("Bronze");
            public static MasteryTier Silver => new MasteryTier("Silver");
            public static MasteryTier Gold => new MasteryTier("Gold");
            public static MasteryTier Platinum => new MasteryTier("Platinum");
            public static MasteryTier Diamond => new MasteryTier("Diamond");
            public static MasteryTier Master => new MasteryTier("Master");
            public static MasteryTier Challenger => new MasteryTier("Challenger");
        }

        /// <summary>
        /// Participant's calculated role
        /// </summary>
        public class Role
        {
            public string Value { get; }
            
            private Role(string value)
            {
                Value = value;
            }
            public static Role BuildFromString(string val)
            {
                var valid = new[]
                {
                    "DUO",
                    "NONE",
                    "SOLO",
                    "DUO_CARRY",
                    "DUO_SUPPORT"
                };
                if (valid.All(s => !string.Equals(s, val, StringComparison.CurrentCultureIgnoreCase)))
                {
                    throw new InvalidEnumArgumentException(val + " role does not exist...");
                }

                return new Role(val);
            }
            
            public static Role Duo => new Role("DUO");
            public static Role None => new Role("NONE");
            public static Role Solo => new Role("SOLO");
            public static Role DuoCarry => new Role("DUO_CARRY");
            public static Role DuoSupport => new Role("DUO_SUPPORT");
        }
        
        public class ConstantNotFoundException : Exception
        {
            public ConstantNotFoundException()
            {
                Console.Error.WriteLine("The specified constant was not found...");
            }

            public ConstantNotFoundException(string message) : base(message)
            {
                Console.Error.WriteLine("The specified constant " + message + " was not found");
            }

            public ConstantNotFoundException(string message, Exception innerException) : base(message, innerException)
            {
                Console.Error.WriteLine("The specified constant " + message + " was not found");
            }
        }
        
        public struct QueueDt
        {
            public int QueueId { get; set; }
            public string Map { get; set; }
            public string Description { get; set; }
            public string Notes { get; set; }
        }
        public struct SeasonDt
        {
            public int Id { get; set; }
            public string Season { get; set; }
        }
        public struct MapDt
        {
            public int MapId { get; set; }
            public string MapName { get; set; }
            public string Notes { get; set; }
        }
        public struct GameModeDt
        {
            public string GameMode { get; set; }
            public string Description { get; set; }
        }
        public struct GameTypeDt
        {
            public string GameType { get; set; }
            public string Description { get; set; }
        }
    }
}