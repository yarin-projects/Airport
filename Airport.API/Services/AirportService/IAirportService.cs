using Airport.API.Models;

namespace Airport.API.Services.AirportService
{
    public interface IAirportService
    {
        Task AddFlightAsync(Flight flight);
        Task ProcessWaitingFlights();
    }
}
