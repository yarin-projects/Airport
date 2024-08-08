using Airport.Http.Client;

namespace Airport.Flight.Simulator.Flights
{
    internal class FlightsTimer
    {
        private static readonly Timer timer = new((state) => CreateFlights());
        private static readonly FlightsHttpClient _service = new();

        internal static void Start()
        {
            Console.ReadKey(true);
            timer.Change(0, 5000);
            Console.ReadKey(true);
        }
        private static void CreateFlights()
        {
            var flight = FlightGenerator.GetNewFlight();
            _service.AddFlight(flight);
            Console.WriteLine($"added flight: {flight.Number}");
        }
    }
}
