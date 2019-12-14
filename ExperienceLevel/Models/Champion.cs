// Bernard Allotey 11-25-2019

using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace ExperienceLevel.Models
{
    public class Champion
    {
        private static Dictionary<string, Champion> _champions;
        
        public string Version { get; set; }
        public string Id { get; set; }
        public string Key { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }
        public string Blurb { get; set; }
        public InfoDt Info { get; set; }
        public ImageDt Image { get; set; }
        public List<string> Tags { get; set; }
        public string ParType { get; set; }
        public StatsDt Stats { get; set; }

        private static Dictionary<string, Champion> RetrieveChamps()
        {
            return JsonConvert
                .DeserializeObject<ChampionListReference>(WebIo.GetChampionsString(GameConstants.GameVersion)).Data;
        }

        public static Dictionary<string, Champion> Champions
        {
            get
            {
                if (_champions != null)
                    return _champions;
                _champions = RetrieveChamps();
                return _champions;
            }
        }

        public static Champion GetChampionByKey(int key)
        {
            foreach (var champion in Champions.Where(champion => int.Parse(champion.Value.Key) == key))
            {
                return champion.Value;
            }
            throw new KeyNotFoundException(key.ToString());
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

        public struct StatsDt
        {
            public double Hp { get; set; }
            public double HpPerLevel { get; set; }
            public double Mp { get; set; }
            public double MpPerLevel { get; set; }
            public double MoveSpeed { get; set; }
            public double Armor { get; set; }
            public double ArmorPerLevel { get; set; }
            public double SpellBlock { get; set; }
            public double SpellBlockPerLevel { get; set; }
            public double AttackRange { get; set; }
            public double HpRegen { get; set; }
            public double HpRegenPerLevel { get; set; }
            public double MpRegen { get; set; }
            public double MpRegenPerLevel { get; set; }
            public double Crit { get; set; }
            public double CritPerLevel { get; set; }
            public double AttackDamage { get; set; }
            public double AttackDamagePerLevel { get; set; }
            public double AttackSpeedPerLevel { get; set; }
            public double AttackSpeed { get; set; }
        }

        public struct ChampionListReference
        {
            public string Type { get; set; }
            public string Format { get; set; }
            public string Version { get; set; }
            public Dictionary<string, Champion> Data { get; set; }
        }
    }
}