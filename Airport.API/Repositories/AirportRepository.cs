using Airport.API.Data;
using Airport.API.Models;
using Airport.API.Models.Enums;
using Airport.API.Services.FlightMovement;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Airport.API.Repositories
{
    public class AirportRepository(AirportContext context, IServiceScopeFactory scopeFactory) : IAirportRepository
    {
        private readonly AirportContext _context = context;
        private readonly IServiceScopeFactory _scopeFactory = scopeFactory;

        public async Task AddFlightAsync(Flight flight)
        {
            _context.Flights.Add(flight);
            await _context.SaveChangesAsync();
        }
        public async Task<IEnumerable<Log>> GetLogsAsync(int lastLogId)
        {
            return await _context.Logs
                .Where(l => l.LogId > lastLogId)
                .ToArrayAsync();
        }
        public async Task<IEnumerable<Leg>> GetLegsAsync()
        {
            return await _context.Legs
                .Include(l => l.Flight)
                .ToArrayAsync();
        }
        public async Task<List<Flight>> GetFlightsAsync()
        {
            var flights = await _context.Flights
                .Include(f => f.Leg)
                .ToListAsync();
            var waitingFlights = await GetWaitingFlightsByLegIdAsync(LegHelpers.UnassignedFlightsLegNumber);
            foreach (var waitingFlight in waitingFlights)
            {
                var flight = ConvertWaitingFlightToFlight(waitingFlight);
                flights.Add(flight);
            }
            return flights;
        }
        public async Task<int> GetMovingFlightsCountAsync()
        {
            return await _context.Legs.CountAsync(l => l.FlightId != null);
        }
        public async Task<Leg> FindLegByIdAsync(int legId)
        {
            return await _context.Legs
                .Include(l => l.Flight)
                .FirstOrDefaultAsync(l => l.LegId == legId);
        }
        public async Task AddLogAsync(Log log)
        {
            _context.Logs.Add(log);
            await _context.SaveChangesAsync();
        }
        public async Task<Leg> SyncLegRowVersionAsync(Leg leg)
        {
            var currentLegFromDb = await _context.Legs
                .AsTracking()
                .FirstOrDefaultAsync(l => l.LegId == leg.LegId);
            _context.Entry(currentLegFromDb).Property(l => l.RowVersion).OriginalValue = leg.RowVersion;
            return currentLegFromDb;
        }
        public Task<IDbContextTransaction> BeginTransactionAsync()
        {
            return _context.Database.BeginTransactionAsync();
        }
        public async Task AddFlightToFirstLeg(Flight flight, Leg firstLeg)
        {
            _context.Legs.Update(firstLeg);
            _context.Flights.Add(flight);
            await _context.SaveChangesAsync();
        }
        public async Task<Leg> FindNextLegAsync(Leg currentLeg)
        {
            var nextLegId = _context.LegConnections
                .Where(lc => lc.LegId == currentLeg.LegId)
                .Select(lc => lc.NextLegId)
                .FirstOrDefault();
            return await FindLegByIdAsync(nextLegId);
        }
        public async Task UpdateFlightAsync(Flight flight)
        {
            _context.Flights.Update(flight);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateFlightAndLegAsync(Flight flight, Leg currentLeg)
        {
            _context.Legs.Update(currentLeg);
            _context.Flights.Update(flight);
            await _context.SaveChangesAsync();
        }
        public async Task<List<WaitingFlight>> GetWaitingFlightsByLegIdAsync(int legId)
        {
            return await _context.WaitingFlights
                .Where(wf => wf.LegIdToEnter == legId)
                .ToListAsync();
        }
        public async Task AddWaitingFlightAsync(WaitingFlight waitingFlight)
        {
            _context.WaitingFlights.Add(waitingFlight);
            await _context.SaveChangesAsync();
        }
        public async Task RemoveWaitingFlightAsync(WaitingFlight waitingFlight)
        {
            var waitingFlightFromDb = await _context.WaitingFlights
                .FirstOrDefaultAsync(wf => wf.WaitingFlightId == waitingFlight.WaitingFlightId);
            if (waitingFlightFromDb != null)
            {
                _context.WaitingFlights.Remove(waitingFlightFromDb);
                await _context.SaveChangesAsync();
            }
        }
        public async Task<Flight> FindFlightByIdAsync(int flightId)
        {
            return await _context.Flights
                .Include(f => f.Leg)
                .FirstOrDefaultAsync(f => f.FlightId == flightId);
        }
        public Flight ConvertWaitingFlightToFlight(WaitingFlight waitingFlight)
        {
            return new Flight
            {
                FlightStatus = waitingFlight.FlightStatus,
                Model = waitingFlight.Model,
                Number = waitingFlight.Number,
                PassengersCount = waitingFlight.PassengersCount,
            };
        }
        public WaitingFlight ConvertFlightToWaitingFlight(Flight flight, int legIdToEnter)
        {
            return new WaitingFlight
            {
                FlightId = flight.FlightId,
                FlightStatus = flight.FlightStatus,
                LegIdToEnter = legIdToEnter,
                Model = flight.Model,
                Number = flight.Number,
                PassengersCount = flight.PassengersCount,
            };
        }
        public async Task CompleteMovingFlights()
        {
            var waitingFlights = await _context.WaitingFlights
                .Where(wf => wf.FlightId != LegHelpers.UnassignedFlightsLegNumber)
                .ToListAsync();
            if (waitingFlights.Count > 0)
            {
                _context.WaitingFlights.RemoveRange(waitingFlights);
                await _context.SaveChangesAsync();
            }

            using var scope = _scopeFactory.CreateScope();
            var service = scope.ServiceProvider.GetRequiredService<IFlightMovementService>();
            var movingFlights = await _context.Flights
                .Where(f => !f.IsDone)
                .ToListAsync();
            foreach (var flight in movingFlights)
            {
                flight.Leg = await FindLegByIdAsync(flight.LegId.Value);
                await service.CompleteFlightAsync(flight, flight.Leg, this);
            }
        }
        public bool EnsureMigrated()
        {
            var databaseExisted = _context.Database.CanConnect();

            _context.Database.Migrate();

            return !databaseExisted;
        }
        public async Task ResetDatabase()
        {
            AirportStatus.Status = AirportAvailability.Closed;
            bool airportFreeForReset = false;
            while (!airportFreeForReset)
            {
                airportFreeForReset = LegHelpers.isLegOccupied.Values.All(isOccupied => !isOccupied);

                await Task.Delay(TimeSpan.FromSeconds(5));
            }
            await Task.Delay(TimeSpan.FromSeconds(5));
            _context.Database.EnsureDeleted();
            EnsureMigrated();
            AirportStatus.Status = AirportAvailability.Open;
        }
    }
}
