using System;
using System.Collections.Generic;
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
            
        }
    }
}
