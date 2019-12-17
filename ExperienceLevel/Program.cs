using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using ExperienceLevel.Models;
using static ExperienceLevel.GeneralIo;
// ReSharper disable ParameterTypeCanBeEnumerable.Local
// ReSharper disable ReturnTypeCanBeEnumerable.Local

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
            //GeneralIo is a static class that just performs the various api functions
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
            //TODO call the overload
            WorstMatchup("covfefebeans");
        }

        static void WinRate(string champ, string user)
        {
            var mySummoner = Summoner.FromJson(GetSummonerString(user));
            var champList = Champion.Champions;
            //According to the RIOT api, this should return all games, up to 100 that had hecarim in it according to
            //my account id
            var matchList = MatchList.FromJson(GetMatchListString(mySummoner.AccountId, 
                new List<Champion> {champList[champ]}));
            var amountOfGames = matchList.TotalGames;
            var champId = int.Parse(champList[champ].Key);
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
                    var match = Match.FromJson(GetMatchString(matchReference.GameId));
                    Console.WriteLine(" " + (match.Participants.Find(participant => 
                                          participant.ChampionId == champId).Stats.Win ? "Win!": "Loss:("));
                    gameList.Add(Match.FromJson(GetMatchString(matchReference.GameId)));
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
                .Where(participant => participant.ChampionId == int.Parse(Champion.Champions[champ].Key))))
            {
                if (participant.Stats.Win)
                {
                    amountWon++;
                }
            }
            stopWatch.Stop();
            Console.WriteLine("Took " + stopWatch.ElapsedMilliseconds/1000 +  " seconds");
            Console.WriteLine("Data for: " + mySummoner.Name);
            Console.WriteLine("Played these many games as "+ champ+ ": " + amountOfGames);
            Console.WriteLine("Won these many games as "+ champ+ ": " + amountWon);
            Console.WriteLine("WR: " + amountWon/amountOfGames * 100 + "%");
        }

        static void WinRateByRole(string role)
        {
            var mySummoner = Summoner.FromJson(GetSummonerString("DaddyFishnets"));
            var champList = Champion.Champions;
            const string champ = "Lux";
            //According to the RIOT api, this should return all games, up to 100 that had lux in it according to
            //my account id
            var matchList = MatchList.FromJson(GetMatchListString(mySummoner.AccountId, 
                new List<Champion> {champList[champ]}));
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
                    if (matchReference.Lane == role)
                    {
                        gameList.Add(Match.FromJson(GetMatchString(matchReference.GameId)));    
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
                where participant.ChampionId == int.Parse(champList[champ].Key) 
                where participant.Stats.Win select participant)
            {
                amountWon++;
            }
            stopWatch.Stop();
            Console.WriteLine("Took " + stopWatch.ElapsedMilliseconds/1000 +  " seconds");
            Console.WriteLine("Data for: " + mySummoner.Name);
            Console.WriteLine("Played these many games as "+ champ+ "in " + role+ ": " + gameList.Count);
            Console.WriteLine("Won these many games as "+ champ+ ": " + amountWon);
            Console.WriteLine("WR: " + amountWon/gameList.Count * 100 + "%");
        }

        static void MatchupStats(string user)
        {
            var summoner = RetrieveSummoner(user);
            var matches = GetMatches(summoner, queues: new List<int> {400, 420, 430, 440});
            var championsPlayedAgainst = CompileOpposingMatchupData(matches, summoner);
            Console.WriteLine("You have played against {0} different champions in the last {1} games.", 
                championsPlayedAgainst.Count, matches.Count);
            var (key, value) = SortChamps(championsPlayedAgainst).ToImmutableList()[0];
            Console.WriteLine("Data for summoner: {0}", summoner.Name);
            Console.WriteLine("Of these, {0} was the most common champion, with a {1}% wr of {2} games", 
                key.Name, 
                (double)value["W"]/value["GP"] * 100,
                value["GP"]);
            
        }

        static void WorstMatchup(string user)
        {
            var summoner = RetrieveSummoner(user);
            var data = CompileOpposingMatchupData(GetMatches(summoner, queues: new List<int>{400, 420, 430, 440}), 
                summoner);
            var highestWr = (default(Champion), 0.0);
            var lowestWr = (default(Champion), 0.0);
            Console.WriteLine("Data for : {0}", summoner.Name);
            foreach (var (key, value) in data)
            {
                var wr = value["W"] / value["GP"];
                Console.WriteLine("Against the champion {0}, you have a {1}% win rate in {2} matches", key.Name, wr*100, 
                    value["GP"]);
                if (wr > highestWr.Item2)
                {
                    highestWr = (key, wr);
                }else if (wr < lowestWr.Item2)
                {
                    lowestWr = (key, wr);
                }
            }
        }

        static void WorstMatchup(string user, Champion champion)
        {
            var summoner = RetrieveSummoner(user);
            var data = CompileOpposingMatchupData(GetMatches(summoner, queues: new List<int>{400, 420, 430, 440}), 
                summoner);
            Console.WriteLine("Data for : {0}", summoner.Name);
            Console.WriteLine("Against {0}, you have a {1}% win rate in {2} games", champion.Name, 
                data[champion]["W"]/data[champion]["GP"], data[champion]["GP"]);
        }
        
        private static List<Match> GetMatches(Summoner summoner, List<Champion> champions = null, 
            List<int> queues = null, long endTime = 0, long beginTime = 0, int endIndex = 0, int beginIndex = 0)
        {
            var matches = new List<Match>();
            var i = 1;
            foreach (var matchReference in MatchList.FromJson(GetMatchListString(summoner.AccountId, champions, queues,
                endTime, beginTime, endIndex, beginIndex)).Matches)
            {
                try
                {
                    Console.WriteLine("Reading Match: " + i);
                    matches.Add(Match.FromJson(GetMatchString(matchReference.GameId)));
                }
                catch (HttpRequestException)
                {
                    Console.WriteLine("Read too many games...");
                    break;
                }
                i++;
            }

            return matches;
        }

        private static Dictionary<Champion, Dictionary<string, double>> SortChamps(
            Dictionary<Champion, Dictionary<string, double>> list)
        {
            var sortedList = new Dictionary<Champion, Dictionary<string, double>>();
            var listCopy = list.ToDictionary(pair => pair.Key, pair => pair.Value);
            while (true)
            {
                if (listCopy.Count == 0)
                {
                    break;
                }
                double[] max = {0, 0};
                foreach (var yeet in listCopy.Where(yeet => yeet.Value["GP"] > max[1]))
                {
                    max[1] = yeet.Value["GP"];
                    max[0] = int.Parse(yeet.Key.Key);
                }
                var champFromKey = Champion.GetChampionByKey((int) max[0]);
                sortedList.Add(champFromKey, listCopy[champFromKey]);
                listCopy.Remove(champFromKey);
            }
            return sortedList;
        }

        private static Dictionary<Champion, Dictionary<string, double>> CompileOpposingMatchupData(List<Match> matches
            , Summoner summoner)
        {
            var championsPlayedAgainst = new Dictionary<Champion, Dictionary<string, double>>();
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
                            new Dictionary<string, double> {{"GP", 1}});
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

            return championsPlayedAgainst.OrderBy(pair => pair.Key.Name)
                .ToDictionary(pair => pair.Key, pair => pair.Value);
        }
    }
}
