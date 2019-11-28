// Bernard Allotey 11-25-2019

using System;
using System.Collections.Generic;

namespace ExperienceLevel.Models
{
    public class Champion
    {
        private static List<Champion> _champions;

        public static List<Champion> RetrieveChamps()
        {
            throw new NotImplementedException();
        }

        public static List<Champion> Champions
        {
            get
            {
                if (_champions.Count < 1)
                    throw new ChampionsNotRetrievedException();
                return _champions;
            }
        }

        private class ChampionsNotRetrievedException : Exception
        {
            public ChampionsNotRetrievedException()
            {
                Console.Error.WriteLine("Must have instanced the champions first...");
            }

            public ChampionsNotRetrievedException(string message) : base(message)
            {
                Console.Error.WriteLine("Must have instanced the champions first...");
            }

            public ChampionsNotRetrievedException(string message, Exception innerException) : base(message, innerException)
            {
                Console.Error.WriteLine("Must have instanced the champions first...");
            }
        }
    }
}