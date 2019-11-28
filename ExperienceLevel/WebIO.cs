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

        private static HttpClient InitializeClient()
        {
            var apiKey = GetApiKey();
            _client = new HttpClient
            {
                BaseAddress = new Uri("https://na1.api.riotgames.com/lol/")
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

        public static string GetMatchListString(string summonerId, HashSet<Champion> champions = null, HashSet<int> queue = null,
            long endTime = 0,
            long beginTime = 0, int endIndex = 0, int beginIndex = 0)
        {
            _client = InitializeClient();
            //TODO Somehow make this so that it actually can handle the parameters
            var response = _client.GetAsync("match/v4/matchlists/by-account/" + summonerId).Result;
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