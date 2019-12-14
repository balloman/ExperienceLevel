using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using ExperienceLevel.Models;

// ReSharper disable ArrangeTypeMemberModifiers
// ReSharper disable ArrangeTypeModifiers

namespace ExperienceLevel
{
    // ReSharper disable once ClassNeverInstantiated.Global
    class Program
    {
        static void Main(string[] args)
        {
            //Create a new summoner object
            //WebIo is a static class that just performs the various api functions
            /*All values from the riot api are returned as strings and consequently all the Web
              Io functions also return strings, however to counteract this, most of the Models
              have static functions that convert from these strings to objects
              */
            /*
             * For example, here is a summoner string
             * {
                    "profileIconId": 1154,
                    "name": "balloman",
                    "puuid": "Iu9a99xPSOp-xXtxEk-wynRVBhF-sNp2vhv2tgNszQHx8PlCOv2G2ChMykS16QsgQRgs4ArzpQUekQ",
                    "summonerLevel": 197,
                    "accountId": "-TV57qA5F6gHBhNggYvncdNG2Lm_3OzyRHVIdQqD4Og0iUw",
                    "id": "IEOr08Sc_mcqV2rcahSialVLB52II8GwoJxknwMze-EGHhE",
                    "revisionDate": 1576001044000
                }
             */
            /*
             * This projects makes use of a library called Json.NET to parse all the strings into objects
             */
            /*
             * Var is just a convention in c# that allows you to skip defining the type of a variable
             * If it is implied in the function being called e.g. var myString = "Hello";, obviously
             * the type of myString is a string so don't need to explicitly define that
             */
            MatchupStats("covfefebeans");
        }

        static void WinRate(string CHAMP, string user)
        {
            var mySummoner = Summoner.FromJson(WebIo.GetSummonerString(user));
            var champList = Champion.Champions;
            //According to the RIOT api, this should return all games, up to 100 that had hecarim in it according to
            //my account id
            var matchList = MatchList.FromJson(WebIo.GetMatchListString(mySummoner.AccountId, 
                new List<Champion> {champList[CHAMP]}));
            var amountOfGames = matchList.TotalGames;
            var champId = int.Parse(champList[CHAMP].Key);
            var amountWon = 0.0;
            var gameList = new List<Match>();
            var i = 0;
            var stopWatch = new Stopwatch();
            stopWatch.Start();
            foreach (var matchReference in matchList.Matches)
            {
                Console.Write("Reading Match: " + i);
                try
                {
                    var match = Match.FromJson(WebIo.GetMatchString(matchReference.GameId));
                    Console.WriteLine(" " + (match.Participants.Find(participant => 
                                          participant.ChampionId == champId).Stats.Win ? "Win!": "Loss:("));
                    gameList.Add(Match.FromJson(WebIo.GetMatchString(matchReference.GameId)));
                }
                catch (HttpRequestException)
                {
                    Console.WriteLine("Reached too many games...");
                    amountOfGames = i + 1;
                    break;
                }
                i++;
            }
            foreach (var participant in gameList.SelectMany(match => match.Participants
                .Where(participant => participant.ChampionId == int.Parse(Champion.Champions[CHAMP].Key))))
            {
                if (participant.Stats.Win)
                {
                    amountWon++;
                }
            }
            stopWatch.Stop();
            Console.WriteLine("Took " + stopWatch.ElapsedMilliseconds/1000 +  " seconds");
            Console.WriteLine("Data for: " + mySummoner.Name);
            Console.WriteLine("Played these many games as "+ CHAMP+ ": " + amountOfGames);
            Console.WriteLine("Won these many games as "+ CHAMP+ ": " + amountWon);
            Console.WriteLine("WR: " + amountWon/amountOfGames * 100 + "%");
        }

        static void WinRateByRole(string ROLE)
        {
            var mySummoner = Summoner.FromJson(WebIo.GetSummonerString("DaddyFishnets"));
            var champList = Champion.Champions;
            const string CHAMP = "Lux";
            //According to the RIOT api, this should return all games, up to 100 that had hecarim in it according to
            //my account id
            var matchList = MatchList.FromJson(WebIo.GetMatchListString(mySummoner.AccountId, 
                new List<Champion> {champList[CHAMP]}));
            var amountWon = 0.0;
            var gameList = new List<Match>();
            var stopWatch = new Stopwatch();
            stopWatch.Start();
            var i = 0;
            foreach (var matchReference in matchList.Matches)
            {
                Console.WriteLine("Reading Match: " + i);
                try
                {
                    if (matchReference.Lane == ROLE)
                    {
                        gameList.Add(Match.FromJson(WebIo.GetMatchString(matchReference.GameId)));    
                    }
                }
                catch (HttpRequestException)
                {
                    Console.WriteLine("Reached too many games...");
                    break;
                }
                i++;
            }

            foreach (var participant 
                in from match 
                    in gameList from participant in match.Participants 
                where participant.ChampionId == int.Parse(champList[CHAMP].Key) 
                where participant.Stats.Win select participant)
            {
                amountWon++;
            }
            stopWatch.Stop();
            Console.WriteLine("Took " + stopWatch.ElapsedMilliseconds/1000 +  " seconds");
            Console.WriteLine("Data for: " + mySummoner.Name);
            Console.WriteLine("Played these many games as "+ CHAMP+ "in " + ROLE+ ": " + gameList.Count);
            Console.WriteLine("Won these many games as "+ CHAMP+ ": " + amountWon);
            Console.WriteLine("WR: " + amountWon/gameList.Count * 100 + "%");
        }

        static void MatchupStats(string user)
        {
            var summoner = Summoner.FromJson(WebIo.GetSummonerString(user));
            var matches = new List<Match>();
            var i = 1;
            foreach (var matchReference in MatchList.FromJson(WebIo.GetMatchListString(summoner.AccountId,
                queues: new List<int>{400, 420, 430, 440})).Matches)
            {
                try
                {
                    Console.WriteLine("Reading Match: " + i);
                    matches.Add(Match.FromJson(WebIo.GetMatchString(matchReference.GameId)));
                }
                catch (HttpRequestException)
                {
                    Console.WriteLine("Read too many games...");
                    break;
                }
                i++;
            }
            var championsPlayedAgainst = new Dictionary<Champion, Dictionary<string, int>>();
            // ReSharper disable once ForeachCanBePartlyConvertedToQueryUsingAnotherGetEnumerator
            foreach (var match in matches)
            {
                // ReSharper disable once ForeachCanBePartlyConvertedToQueryUsingAnotherGetEnumerator
                foreach (var participant in match.Participants)
                {
                    var playerTeam = match.Participants.Find(participant1 => match.ParticipantIdentities.Find(
                                                                                     identity =>
                                                                                         identity.Player.AccountId ==
                                                                                         summoner.AccountId)
                                                                                 .ParticipantId ==
                                                                             participant1.ParticipantId).TeamId;
                    if (participant.TeamId == playerTeam) continue;
                    var champ = Champion.GetChampionByKey(participant.ChampionId);
                    if (!championsPlayedAgainst.ContainsKey(champ))
                    {
                        championsPlayedAgainst.Add(champ,
                            new Dictionary<string, int> {{"GP", 1}});
                        if (match.Teams.Find(stats => stats.TeamId == playerTeam).Win.Equals("Win"))
                        {
                            championsPlayedAgainst[champ].Add("W", 1);
                        }
                        else
                        {
                            championsPlayedAgainst[champ].Add("W", 0);
                        }
                    }
                    else
                    {
                        championsPlayedAgainst[Champion.GetChampionByKey(participant.ChampionId)]["GP"]++;
                        if (match.Teams.Find(stats => stats.TeamId == playerTeam).Win.Equals("Win"))
                        {
                            championsPlayedAgainst[champ]["W"]++;
                        }
                    }
                }
            }

            
            
            Console.WriteLine("You have played against {0} different champions in the last {1} games.", 
                championsPlayedAgainst.Count, i);
            var mostCommon = SortChamps(championsPlayedAgainst).ToImmutableList()[0];
            Console.WriteLine("Data for summoner: {0}", summoner.Name);
            Console.WriteLine("Of these, {0} was the most common champion, with a {1}% wr of {2} games", 
                mostCommon.Key.Name, 
                (double)mostCommon.Value["W"]/mostCommon.Value["GP"] * 100,
                mostCommon.Value["GP"]);
            
        }

        private static Dictionary<Champion, Dictionary<string, int>> SortChamps(
            Dictionary<Champion, Dictionary<string, int>> list)
        {
            var sortedList = new Dictionary<Champion, Dictionary<string, int>>();
            var listCopy = list.ToDictionary(pair => pair.Key, pair => pair.Value);
            while (true)
            {
                if (listCopy.Count == 0)
                {
                    break;
                }
                int[] max = {0, 0};
                foreach (var yeet in listCopy.Where(yeet => yeet.Value["GP"] > max[1]))
                {
                    max[1] = yeet.Value["GP"];
                    max[0] = int.Parse(yeet.Key.Key);
                }
                var champFromKey = Champion.GetChampionByKey(max[0]);
                sortedList.Add(champFromKey, listCopy[champFromKey]);
                listCopy.Remove(champFromKey);
            }
            return sortedList;
        }
    }
}
