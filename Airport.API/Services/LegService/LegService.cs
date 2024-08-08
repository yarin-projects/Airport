using Airport.API.Models;
using Airport.API.Models.Enums;
using Airport.API.Repositories;

namespace Airport.API.Services.LegService
{
    public class LegService : ILegService
    {
        private const bool departureFlight = true;
        public async Task<bool> DoMovingFlightsExceedThresholdAsync(IAirportRepository repository)
        {
            var movingFlights = await repository.GetMovingFlightsCountAsync();
            return movingFlights >= LegHelpers.maxMovingFlightsAcrossLegs;
        }
        public async Task<Leg> GetStartingLegAsync(IAirportRepository repository, Flight flight)
        {
            return flight.FlightStatus switch
            {
                FlightStatus.Landing => await repository.FindLegByIdAsync(LegIds.Leg1),
                FlightStatus.Departure => await GetLeg6Or7Async(repository, departureFlight),
                _ => null
            };
        }
        public async Task<Leg> GetNextLegAsync(Flight flight, IAirportRepository repository, Leg currentLeg)
        {
            return flight.FlightStatus switch
            {
                FlightStatus.Landing => await HandleArrivalFlightAsync(currentLeg, repository),
                FlightStatus.Departure => await HandleDepartureFlightAsync(currentLeg, repository),
                _ => null
            };
        }
        public async Task<Leg> GetLeg6Or7Async(IAirportRepository repository, bool isDeparture)
        {
            var leg6 = await repository.FindLegByIdAsync(LegIds.Leg6);
            var leg7 = await repository.FindLegByIdAsync(LegIds.Leg7);
            var isLeg6Occupied = LegHelpers.isLegOccupied[leg6.LegId];
            var isLeg7Occupied = LegHelpers.isLegOccupied[leg7.LegId];

            if (!isLeg6Occupied && !isLeg7Occupied)
            {
                return leg6.CrossingTime.TotalSeconds < leg7.CrossingTime.TotalSeconds ? leg6 : leg7;
            }
            if (isDeparture)
            {
                if (!isLeg6Occupied && isLeg7Occupied && leg7.Flight?.FlightStatus == FlightStatus.Landing)
                {
                    return leg6;
                }
                else if (!isLeg7Occupied && isLeg6Occupied && leg6.Flight?.FlightStatus == FlightStatus.Landing)
                {
                    return leg7;
                }
                else
                {
                    return null;
                }
            }
            else
            {
                if (!isLeg6Occupied)
                {
                    return leg6;
                }
                if (!isLeg7Occupied)
                {
                    return leg7;
                }
            }
            return await CompareWaitingFlightsLeg6AndLeg7Async(repository) ? leg6 : leg7;
        }
        public async Task<bool> CheckLegAvailabilityAsync(IAirportRepository repository, Leg leg)
        {
            using var transaction = await repository.BeginTransactionAsync();
            try
            {
                var legFromDb = await repository.FindLegByIdAsync(leg.LegId);

                if (legFromDb == null || legFromDb.FlightId.HasValue)
                {
                    return false;
                }
                var isEqual = leg.RowVersion.SequenceEqual(legFromDb.RowVersion);

                await transaction.CommitAsync();

                return isEqual;
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
        private static async Task<bool> CompareWaitingFlightsLeg6AndLeg7Async(IAirportRepository repository)
        {
            var leg6 = await repository.GetWaitingFlightsByLegIdAsync(LegIds.Leg6);
            var leg7 = await repository.GetWaitingFlightsByLegIdAsync(LegIds.Leg7);
            return leg6.Count < leg7.Count;
        }
        private async Task<Leg> HandleArrivalFlightAsync(Leg currentLeg, IAirportRepository repository)
        {
            return currentLeg.LegId switch
            {
                LegIds.Leg4 => await repository.FindLegByIdAsync(LegIds.Leg5),
                LegIds.Leg5 => await GetLeg6Or7Async(repository, !departureFlight),
                LegIds.Leg6 or LegIds.Leg7 => null,
                _ => await repository.FindNextLegAsync(currentLeg)
            };
        }
        private static async Task<Leg> HandleDepartureFlightAsync(Leg currentLeg, IAirportRepository repository)
        {
            return currentLeg.LegId switch
            {
                LegIds.Leg4 => await repository.FindLegByIdAsync(LegIds.Leg9),
                LegIds.Leg9 => null,
                _ => await repository.FindNextLegAsync(currentLeg)
            };
        }
    }
}
