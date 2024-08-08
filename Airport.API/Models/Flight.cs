using Airport.API.Models.Enums;

namespace Airport.API.Models
{
    public class Flight
    {
        public int FlightId { get; set; }
        public Guid Number { get; set; }
        public int PassengersCount { get; set; }
        public string Model { get; set; }
        public FlightStatus FlightStatus { get; set; }
        public bool IsDone { get; set; } = false;
        public DateTimeOffset TimeCompleted { get; set; }
        public int? LegId { get; set; }
        public virtual Leg Leg { get; set; }
    }
}
