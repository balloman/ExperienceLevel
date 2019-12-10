// Bernard Allotey 11-25-2019

using System;
using System.Collections.Generic;
using System.IO;
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

        public static string GetMatchListString(string accountId, HashSet<Champion> champions = null, HashSet<int> queue = null,
            long endTime = 0,
            long beginTime = 0, int endIndex = 0, int beginIndex = 0)
        {
            _client = InitializeClient();
            //TODO Somehow make this so that it actually can handle the parameters
            var response = _client.GetAsync("match/v4/matchlists/by-account/" + accountId).Result;
            string responseString;
            if (response.IsSuccessStatusCode)
            {
                responseString = response.Content.ReadAsStringAsync().Result;
            }
            else
            {
                throw new HttpRequestException("Error Code: " + response.StatusCode);
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
            var response = _client.GetAsync("match/v4/matches/" + matchId).Result;
            if (response.IsSuccessStatusCode)
            {
                _client.Dispose();
                return response.Content.ReadAsStringAsync().Result;
            }
            _client.Dispose();
            throw new HttpRequestException("Error Code: " + response.StatusCode);
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