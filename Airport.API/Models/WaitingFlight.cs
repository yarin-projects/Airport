using Airport.API.Models.Enums;

namespace Airport.API.Models
{
    public class WaitingFlight
    {
        public int WaitingFlightId { get; set; }
        public int FlightId { get; set; }
        public int LegIdToEnter { get; set; }
        public Guid Number { get; set; }
        public int PassengersCount { get; set; }
        public string Model { get; set; }
        public FlightStatus FlightStatus { get; set; }
    }
}
