using Airport.API.Models;
using Airport.API.Models.Enums;
using Airport.API.Repositories;
using Airport.API.Services.FlightMovement;
using Airport.API.Services.LegService;

namespace Airport.API.Services.AirportService
{
    public class AirportService(IServiceScopeFactory serviceScopeFactory, IFlightMovementService movementService, ILegService legService) : IAirportService
    {
        private readonly IServiceScopeFactory _serviceScopeFactory = serviceScopeFactory;
        private readonly IFlightMovementService _flightMovementService = movementService;
        private readonly ILegService _legService = legService;
        private const bool departureFlight = true;

        public async Task AddFlightAsync(Flight flight)
        {
            using var scope = _serviceScopeFactory.CreateScope();
            var repository = scope.ServiceProvider.GetRequiredService<IAirportRepository>();

            if (await _legService.DoMovingFlightsExceedThresholdAsync(repository) || 
                AirportStatus.Status == AirportAvailability.Closed)
            {
                await AddFlightToWaitingListAndLog(repository, flight, LegHelpers.UnassignedFlightsLegNumber);
                return;
            }

            var startingLeg = await _legService.GetStartingLegAsync(repository, flight);

            if (startingLeg == null || LegHelpers.isLegOccupied[startingLeg.LegId])
            {
                await AddFlightToWaitingListAndLog(repository, flight, LegHelpers.UnassignedFlightsLegNumber);
                return;
            }

            await _flightMovementService.MoveFlightToFirstLegAsync(flight, startingLeg, repository);

            await ProcessFlightRouteAsync(flight);
        }
        private async Task ProcessFlightRouteAsync(Flight flight)
        {
            using var scope = _serviceScopeFactory.CreateScope();
            var repository = scope.ServiceProvider.GetRequiredService<IAirportRepository>();

            Leg currentLeg = flight.Leg;

            while (currentLeg != null && !flight.IsDone)
            {
                await Task.Delay(currentLeg.CrossingTime);

                Leg nextLeg = await _legService.GetNextLegAsync(flight, repository, currentLeg);

                if (nextLeg == null)
                {
                    await _flightMovementService.CompleteFlightAsync(flight, currentLeg, repository);
                    return;
                }
                else if (LegHelpers.isLegOccupied[nextLeg.LegId])
                {
                    await AddFlightToWaitingListAndLog(repository, flight, nextLeg.LegId);
                    return;
                }

                await _flightMovementService.MoveFlightToNextLegAsync(flight, currentLeg, nextLeg, repository);

                await ProcessFlightsInQueue(currentLeg.LegId);

                currentLeg = nextLeg;
            }
        }
        public async Task ProcessWaitingFlights()
        {
            for (int legId = LegHelpers.UnassignedFlightsLegNumber; legId <= LegHelpers.legsCount; legId++)
            {
                await ProcessFlightsInQueue(legId);
            }
        }
        private async Task ProcessFlightsInQueue(int legId)
        {
            if (AirportStatus.Status == AirportAvailability.Closed && legId == LegHelpers.UnassignedFlightsLegNumber)
            {
                return;
            }
            using var scope = _serviceScopeFactory.CreateScope();
            var repository = scope.ServiceProvider.GetRequiredService<IAirportRepository>();

            var waitingFlights = await repository.GetWaitingFlightsByLegIdAsync(legId);

            foreach (var waitingFlight in waitingFlights)
            {
                bool success = legId == LegHelpers.UnassignedFlightsLegNumber
                    ? await AssignUnassignedFlightAsync(waitingFlight, repository)
                    : await AssignFlightToLegAsync(waitingFlight, legId, repository);

                if (!success)
                {
                    break;
                }
            }
        }
        private async Task<bool> AssignUnassignedFlightAsync(WaitingFlight waitingFlight, IAirportRepository repository)
        {
            var flight = repository.ConvertWaitingFlightToFlight(waitingFlight);

            Leg startingLeg = await _legService.GetStartingLegAsync(repository, flight);

            if (startingLeg == null || !await _legService.CheckLegAvailabilityAsync(repository, startingLeg))
            {
                return false;
            }

            if (!LegHelpers.isLegOccupied[startingLeg.LegId] && !await _legService.DoMovingFlightsExceedThresholdAsync(repository))
            {
                await repository.RemoveWaitingFlightAsync(waitingFlight);
                await _flightMovementService.MoveFlightToFirstLegAsync(flight, startingLeg, repository);
                await ProcessFlightRouteAsync(flight);
                return true;
            }
            else
            {
                return false;
            }
        }
        private async Task<bool> AssignFlightToLegAsync(WaitingFlight waitingFlight, int legId, IAirportRepository repository)
        {
            var flight = await repository.FindFlightByIdAsync(waitingFlight.FlightId);

            Leg leg;
            if (flight.LegId == 5)
            {
                leg = await _legService.GetLeg6Or7Async(repository, !departureFlight);
            }
            else
            {
                leg = await repository.FindLegByIdAsync(legId);
            }

            if (leg == null || !await _legService.CheckLegAvailabilityAsync(repository, leg))
            {
                return false;
            }

            if (!LegHelpers.isLegOccupied[leg.LegId])
            {
                await repository.RemoveWaitingFlightAsync(waitingFlight);
                await _flightMovementService.MoveFlightToNextLegAsync(flight, flight.Leg, leg, repository);
                await ProcessFlightRouteAsync(flight);
                return true;
            }
            else
            {
                return false;
            }
        }
        private async Task AddFlightToWaitingListAndLog(IAirportRepository repository, Flight flight, int legIdToEnter)
        {
            var waitingFlight = repository.ConvertFlightToWaitingFlight(flight, legIdToEnter);

            await repository.AddWaitingFlightAsync(waitingFlight);

            if (legIdToEnter == 0)
            {
                var leg = await _legService.GetStartingLegAsync(repository, flight);
                if (leg == null)
                {
                    legIdToEnter = LegIds.Leg6;
                }
                else
                {
                    legIdToEnter = leg.LegId;
                }
            }
            var log = new Log
            {
                FlightId = flight.FlightId,
                LegId = legIdToEnter,
                IsWaitingList = true,
                FlightNumber = flight.Number.ToString(),
                TimeEntered = DateTimeOffset.UtcNow,
            };
            await repository.AddLogAsync(log);
        }
    }
}
