using Airport.API.Models.Enums;

namespace Airport.API.Models
{
    public static class AirportStatus
    {
        public static AirportAvailability Status { get; set; } = AirportAvailability.Open;
    }
}
