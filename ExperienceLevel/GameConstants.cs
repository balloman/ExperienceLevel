// Bernard Allotey 11-26-2019

using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;

namespace ExperienceLevel
{
    public static class GameConstants
    {
        public static bool Instanced { get; private set; } = false;
        private static List<SeasonDt> _seasons;
        private static List<QueueDt> _queues;
        private static List<MapDt> _maps;
        private static List<GameModeDt> _gameModes;
        private static List<GameTypeDt> _gameTypes;

        /// <summary>
        /// Should attempt to lookup the jsons for all of the types<para></para>
        /// If not called, attempts to lookup any of the values will result in a <exception cref="NotInstantiatedException"></exception>
        /// </summary>
        public static void LookupLists()
        {
            var client = new HttpClient {BaseAddress = new Uri("http://static.developer.riotgames.com/docs/lol/")};
            Queues = JsonConvert.DeserializeObject<List<QueueDt>>(client.GetAsync("queues.json").Result.Content.ReadAsStringAsync().Result);
            Seasons = JsonConvert.DeserializeObject<List<SeasonDt>>(client.GetAsync("seasons.json").Result.Content.ReadAsStringAsync().Result);
            Maps = JsonConvert.DeserializeObject<List<MapDt>>(client.GetAsync("maps.json").Result.Content.ReadAsStringAsync().Result);
            GameModes = JsonConvert.DeserializeObject<List<GameModeDt>>(client.GetAsync("gameModes.json").Result.Content.ReadAsStringAsync().Result);
            GameTypes = JsonConvert.DeserializeObject<List<GameTypeDt>>(client.GetAsync("gameTypes.json").Result.Content.ReadAsStringAsync().Result);
            Instanced = true;
            client.Dispose();
        }

        public static List<SeasonDt> Seasons
        {
            get {
                if (Instanced)
                {
                    return _seasons;
                }
                else
                {
                    throw new NotInstantiatedException();
                }
            }
            private set => _seasons = value;
        }
        public static List<QueueDt> Queues
        {
            get
            {
                if (Instanced)
                {
                    return _queues;
                }
                else
                {
                    throw new NotInstantiatedException();
                }
            }
            private set => _queues = value;
        }
        public static List<MapDt> Maps
        {
            get
            {
                if (Instanced)
                {
                    return _maps;
                }
                else
                {
                    throw new NotInstantiatedException();
                }
            }
            private set => _maps = value;
        }
        public static List<GameModeDt> GameModes
        {
            get
            {
                if (Instanced)
                {
                    return _gameModes;
                }
                else
                {
                    throw new NotInstantiatedException();
                }
            }
            private set => _gameModes = value;
        }
        public static List<GameTypeDt> GameTypes
        {
            get
            {
                if (Instanced)
                {
                    return _gameTypes;
                }
                else
                {
                    throw new NotInstantiatedException();
                }
            }
            private set => _gameTypes = value;
        }

        public class MasteryTier
        {
            private
        }

        public class NotInstantiatedException:Exception
        {
            public NotInstantiatedException()
            {
                Console.Error.WriteLine("You must make a successful call to LookupLists before attempting to use this!");
            }

            public NotInstantiatedException(string message) : base(message)
            {
                Console.Error.WriteLine("You must make a successful call to LookupLists before attempting to use this!");
            }

            public NotInstantiatedException(string message, Exception innerException) : base(message, innerException)
            {
                Console.Error.WriteLine("You must make a successful call to LookupLists before attempting to use this!");
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
            public string Id { get; set; }
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