// Bernard Allotey 11-25-2019

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using ExperienceLevel.Models;
using Newtonsoft.Json;

namespace ExperienceLevel
{
    public static class WebIo
    {
        private static HttpClient _client;

        private static HttpClient InitializeClient(string baseUrl="https://na1.api.riotgames.com/lol/")
        {
            var apiKey = GetApiKey();
            _client = new HttpClient
            {
                BaseAddress = new Uri(baseUrl)
            };
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            if (string.IsNullOrEmpty(apiKey))
            {
                throw new KeyNotSetException();
            }
            _client.DefaultRequestHeaders.Add("X-Riot-Token", GetApiKey());
            return _client;
        }

        public static string GetSummonerString(string summonerName)
        {
            var apiKey = GetApiKey();
            _client = new HttpClient
            {
                BaseAddress = new Uri("https://na1.api.riotgames.com/lol/summoner/v4/summoners/by-name/")
            };
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            if (string.IsNullOrEmpty(apiKey))
            {
                throw new KeyNotSetException();
            }
            _client.DefaultRequestHeaders.Add("X-Riot-Token", apiKey);
            var response = _client.GetAsync(summonerName).Result;
            var responseString = string.Empty;
            if (response.IsSuccessStatusCode)
            {
                responseString = response.Content.ReadAsStringAsync().Result;
            }
            _client.Dispose();
            return responseString;
        }

        public static string GetMatchListString(string accountId, List<Champion> champions = null, 
            List<int> queues = null,
            long endTime = 0,
            long beginTime = 0, int endIndex = 0, int beginIndex = 0)
        {
            _client = InitializeClient();
            var champEndpoint = "";
            if (champions != null)
            {
                champEndpoint = "champion=" + champions[0].Key;
                var i = 0;
                foreach (var champion in champions)
                {
                    if (i == 0)
                    {
                        i++;
                        continue;
                    }
                    champEndpoint += "&champion=" + champion.Key;
                }
            }

            var queueEndpoint = "";
            if (queues != null)
            {
                queueEndpoint = "queue=" + queues[0];
                var i = 0;
                foreach (var queue in queues)
                {
                    if (i == 0)
                    {
                        i++;
                        continue;
                    }
                    queueEndpoint += "&queue=" + queue;
                }
            }

            var endTimeEndpoint = endTime == 0 ? "" : "&endTime=" + endTime;
            var beginTimeEndpoint = beginTime == 0 ? "" : "&beginTime=" + beginTime;
            var endIndexEndpoint = endIndex == 0 ? "" : "&endIndex=" + endIndex;
            var beginIndexEndpoint = beginIndex == 0 ? "" : "&beginIndex=" + beginIndex;
            var endPoint = "match/v4/matchlists/by-account/" + accountId
                                                             + "?"
                                                             + champEndpoint
                                                             + "&"
                                                             + queueEndpoint
                                                             + endTimeEndpoint
                                                             + beginTimeEndpoint
                                                             + endIndexEndpoint
                                                             + beginIndexEndpoint;
            var response = _client.GetAsync(endPoint).Result;
            string responseString;
            if (response.IsSuccessStatusCode)
            {
                responseString = response.Content.ReadAsStringAsync().Result;
            }
            else
            {
                throw new HttpRequestException("Error Code: " + response.StatusCode + "\n " + endPoint);
            }
            _client.Dispose();
            return responseString;
        }

        /// <summary>
        /// Gets a match from the RIOT Api using a matchId
        /// </summary>
        /// <param name="matchId">The game id to search for</param>
        /// <returns>A string that</returns>
        public static string GetMatchString(long matchId)
        {
            _client = InitializeClient();
            //THe first argument is the url to make the request to, and it does this synchronously so you have to wait
            //for it
            var response = _client.GetAsync("match/v4/matches/" + matchId).Result;
            if (response.IsSuccessStatusCode)
            {
                _client.Dispose();
                return response.Content.ReadAsStringAsync().Result;
            }
            _client.Dispose();
            throw new HttpRequestException("Error Code: " + response.StatusCode); //If we get a 404 or something
            //This will throw
        }

        /// <summary>
        /// Just retrieves the api key from apikey.txt
        /// </summary>
        /// <returns>The API key</returns>
        private static string GetApiKey()
        {
            const string path = "apikey.txt";
            var key = string.Empty;
            using (var streamReader = File.OpenText(path))
            {
                string s;
                while ((s = streamReader.ReadLine()) != null)
                {
                    key += s;
                }
            }
            return key;
        }

        /// <summary>
        /// Gets the version list
        /// </summary>
        /// <returns>The version list</returns>
        /// <exception cref="HttpRequestException">thrown if we don't get a success status code</exception>
        public static string GetVersionsString()
        {
            _client = InitializeClient("https://ddragon.leagueoflegends.com/api/");
            var response = _client.GetAsync("versions.json").Result;
            if (response.IsSuccessStatusCode)
            {
                _client.Dispose();
                return response.Content.ReadAsStringAsync().Result;
            }
            _client.Dispose();
            throw new HttpRequestException("Error Code: " + response.StatusCode);
        }

        /// <summary>
        /// Gets the list of champions
        /// </summary>
        /// <param name="version">The version to check</param>
        /// <returns>The list of champs</returns>
        public static string GetChampionsString(string version)
        {
            _client = InitializeClient("http://ddragon.leagueoflegends.com/cdn/9.23.1/data/en_US/");
            var response = _client.GetAsync("champion.json").Result;
            if (response.IsSuccessStatusCode)
            {
                _client.Dispose();
                return response.Content.ReadAsStringAsync().Result;
            }
            _client.Dispose();
            throw new HttpRequestException("Error code: " + response.StatusCode);
        }

        private class KeyNotSetException : Exception
        {
            public KeyNotSetException()
            {
                Console.Error.WriteLine("API Key must be set!");
            }

            public KeyNotSetException(string message) : base(message)
            {
                Console.Error.WriteLine("API Key must be set!");
            }

            public KeyNotSetException(string message, Exception innerException) : base(message, innerException)
            {
                Console.Error.WriteLine("API Key must be set!");
            }
        }

    }
}