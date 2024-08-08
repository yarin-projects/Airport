using Airport.Http.Client.Models;
using System.Net.Http.Json;

namespace Airport.Http.Client
{
    public class FlightsHttpClient
    {
        private readonly HttpClient _client = new() { BaseAddress = new Uri("http://localhost:5204") };

        public void AddFlight(FlightDto flight) => _client.PostAsJsonAsync("api/post/flight", flight);
    }
}
