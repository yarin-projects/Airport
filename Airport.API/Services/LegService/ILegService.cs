using Airport.API.Models;
using Airport.API.Repositories;

namespace Airport.API.Services.LegService
{
    public interface ILegService
    {
        Task<bool> DoMovingFlightsExceedThresholdAsync(IAirportRepository repository);
        Task<Leg> GetStartingLegAsync(IAirportRepository repository, Flight flight);
        Task<Leg> GetNextLegAsync(Flight flight, IAirportRepository repository, Leg currentLeg);
        Task<Leg> GetLeg6Or7Async(IAirportRepository repository, bool isDeparture);
        Task<bool> CheckLegAvailabilityAsync(IAirportRepository repository, Leg leg);
    }
}
