// Bernard Allotey 11-25-2019

namespace ExperienceLevel.Models
{
    public struct MatchReference
    {
        public string Lane { get; set; }
        public long GameId { get; set; }
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