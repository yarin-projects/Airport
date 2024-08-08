using Airport.Http.Client.Models.Enums;

namespace Airport.Http.Client.Models
{
    public class FlightDto
    {
        public Guid Number { get; set; }
        public int PassengersCount { get; set; }
        public string Model { get; set; }
        public FlightStatusDto FlightStatus { get; set; }
    }
}
