using Airport.API.Models;
using Airport.API.Repositories;

namespace Airport.API.Services.FlightMovement
{
    public class FlightMovementService : IFlightMovementService
    {
        public async Task MoveFlightToFirstLegAsync(Flight flight, Leg startingLeg, IAirportRepository repository)
        {
            using var transaction = await repository.BeginTransactionAsync();
            try
            {
                Leg startingLegFromDb = await repository.SyncLegRowVersionAsync(startingLeg);

                AssignFlightToLeg(flight, startingLegFromDb);

                await repository.AddFlightToFirstLeg(flight, startingLegFromDb);


                var log = new Log
                {
                    FlightId = flight.FlightId,
                    LegId = startingLeg.LegId,
                    IsWaitingList = false,
                    FlightNumber = flight.Number.ToString(),
                    TimeEntered = DateTimeOffset.UtcNow,
                };
                await repository.AddLogAsync(log);

                await transaction.CommitAsync();
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
            }
        }
        public async Task MoveFlightToNextLegAsync(Flight flight, Leg currentLeg, Leg nextLeg, IAirportRepository repository)
        {
            using var transaction = await repository.BeginTransactionAsync();
            try
            {
                await DetachFlightFromCurrentLegAsync(flight, currentLeg, repository);

                Leg nextLegFromDb = await repository.SyncLegRowVersionAsync(nextLeg);

                AssignFlightToLeg(flight, nextLegFromDb);

                await repository.UpdateFlightAndLegAsync(flight, nextLegFromDb);

                var log = new Log
                {
                    FlightId = flight.FlightId,
                    LegId = nextLegFromDb.LegId,
                    IsWaitingList = false,
                    FlightNumber = flight.Number.ToString(),
                    TimeEntered = DateTimeOffset.UtcNow,
                };
                await repository.AddLogAsync(log);

                await transaction.CommitAsync();
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
            }
        }
        public async Task CompleteFlightAsync(Flight flight, Leg currentLeg, IAirportRepository repository)
        {
            using var transaction = await repository.BeginTransactionAsync();
            try
            {
                await DetachFlightFromCurrentLegAsync(flight, currentLeg, repository);

                flight.IsDone = true;
                flight.TimeCompleted = DateTimeOffset.UtcNow;

                await repository.UpdateFlightAsync(flight);

                await transaction.CommitAsync();
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
            }
        }
        private static void AssignFlightToLeg(Flight flight, Leg leg)
        {
            flight.LegId = leg.LegId;
            flight.Leg = leg;

            leg.FlightId = flight.FlightId;
            leg.Flight = flight;

            LegHelpers.isLegOccupied[leg.LegId] = true;
        }
        private static async Task DetachFlightFromCurrentLegAsync(Flight flight, Leg currentLeg, IAirportRepository repository)
        {
            Leg currentLegFromDb = await repository.SyncLegRowVersionAsync(currentLeg);

            currentLegFromDb.FlightId = null;
            currentLegFromDb.Flight = null;

            flight.LegId = null;
            flight.Leg = null;

            await repository.UpdateFlightAndLegAsync(flight, currentLegFromDb);

            LegHelpers.isLegOccupied[currentLegFromDb.LegId] = false;

            var log = new Log
            {
                FlightId = flight.FlightId,
                LegId = currentLeg.LegId,
                IsWaitingList = false,
                FlightNumber = flight.Number.ToString(),
                TimeExit = DateTimeOffset.UtcNow,
            };
            await repository.AddLogAsync(log);
        }
    }
}
