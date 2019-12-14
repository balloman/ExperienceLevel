// Bernard Allotey 11-25-2019

namespace ExperienceLevel.Models
{
    //An individual MatchReference, which isnt the same as a match
    public struct MatchReference
    {
        public string Lane { get; set; }
        public long GameId { get; set; } //This one is important as it allows us to lookup a game using the game id
        public int Champion { get; set; }
        public string PlatformId { get; set; }
        public int Season { get; set; }
        public int Queue { get; set; }
        private GameConstants.Role _role;
        public long Timestamp { get; set; }

        public string Role
        {
            get => _role.Value;
            set => _role = GameConstants.Role.BuildFromString(value);
        }
    }
}