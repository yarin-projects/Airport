using Airport.API.Models;
using Airport.API.Repositories;

namespace Airport.API.Services.FlightMovement
{
    public interface IFlightMovementService
    {
        Task MoveFlightToFirstLegAsync(Flight flight, Leg startingLeg, IAirportRepository repository);
        Task MoveFlightToNextLegAsync(Flight flight, Leg currentLeg, Leg nextLeg, IAirportRepository repository);
        Task CompleteFlightAsync(Flight flight, Leg currentLeg, IAirportRepository repository);
    }
}
