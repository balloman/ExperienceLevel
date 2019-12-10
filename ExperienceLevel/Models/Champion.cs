// Bernard Allotey 11-25-2019

using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace ExperienceLevel.Models
{
    public class Champion
    {
        private static List<Champion> _champions;
        
        public string Version { get; set; }
        public string Id { get; set; }
        public string Key { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }
        public string Blurb { get; set; }
        public InfoDt Info { get; set; }
        
        private static List<Champion> RetrieveChamps()
        {
            return JsonConvert.DeserializeObject<List<Champion>>(WebIo.GetChampionsString(GameConstants.GameVersion));
        }

        public static List<Champion> Champions
        {
            get
            {
                if (_champions.Count < 1)
                    _champions = RetrieveChamps();
                return _champions;
            }
        }
        
        public struct InfoDt
        {
            public int Attack { get; set; }
            public int Defense { get; set; }
            public int Magic { get; set; }
            public int Difficulty { get; set; }
        }

        public struct ImageDt
        {
            public string Full { get; set; }
            public string Sprite { get; set; }
            public string Group { get; set; }
            public double X { get; set; }
            public double Y { get; set; }
            public double W { get; set; }
            public double H { get; set; }
        }
        
    }
}