using Airport.API.Models;
using Microsoft.EntityFrameworkCore.Storage;

namespace Airport.API.Repositories
{
    public interface IAirportRepository
    {
        Task AddFlightAsync(Flight flight);
        Task<IEnumerable<Log>> GetLogsAsync(int lastLogId);
        Task<IEnumerable<Leg>> GetLegsAsync();
        Task<List<Flight>> GetFlightsAsync();
        Task<int> GetMovingFlightsCountAsync();
        Task<Leg> FindLegByIdAsync(int legId);
        Task AddLogAsync(Log log);
        Task<Leg> SyncLegRowVersionAsync(Leg leg);
        Task<IDbContextTransaction> BeginTransactionAsync();
        Task AddFlightToFirstLeg(Flight flight, Leg firstLeg);
        Task<Leg> FindNextLegAsync(Leg currentLeg);
        Task UpdateFlightAsync(Flight flight);
        Task UpdateFlightAndLegAsync(Flight flight, Leg currentLeg);
        Task<List<WaitingFlight>> GetWaitingFlightsByLegIdAsync(int legId);
        Task AddWaitingFlightAsync(WaitingFlight waitingFlight);
        Task RemoveWaitingFlightAsync(WaitingFlight waitingFlight);
        Task<Flight> FindFlightByIdAsync(int flightId);
        Flight ConvertWaitingFlightToFlight(WaitingFlight waitingFlight);
        WaitingFlight ConvertFlightToWaitingFlight(Flight flight, int legIdToEnter);
        Task CompleteMovingFlights();
        bool EnsureMigrated();
        Task ResetDatabase();
    }
}
