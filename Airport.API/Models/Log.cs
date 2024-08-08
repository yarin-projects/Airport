namespace Airport.API.Models
{
    public class Log
    {
        public int LogId { get; set; }
        public int LegId { get; set; }
        public int FlightId { get; set; }
        public string FlightNumber { get; set; }
        public bool IsWaitingList { get; set; }
        public DateTimeOffset TimeEntered { get; set; }
        public DateTimeOffset TimeExit { get; set; }
    }
}
