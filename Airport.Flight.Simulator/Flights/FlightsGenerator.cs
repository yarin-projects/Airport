using Airport.Http.Client.Models;
using Airport.Http.Client.Models.Enums;

namespace Airport.Flight.Simulator.Flights
{
    internal class FlightGenerator
    {
        public static FlightDto GetNewFlight()
        {
            var flightModels = new List<(string Model, (int Min, int Max) PassengerRange)>
        {
            ("Airbus A320", (140, 171)),
            ("Boeing 737", (162, 190)),
            ("Boeing 777", (301, 451)),
            ("Airbus A330", (201, 246)),
            ("Embraer E-Jet family", (51, 101)),
            ("Boeing 787 Dreamliner", (210, 251)),
            ("Airbus A350 XWB", (300, 411))
        };
            var (model, passengerRange) = flightModels[Random.Shared.Next(flightModels.Count)];

            return CreateFlightDto(model, passengerRange);
        }
        private static FlightDto CreateFlightDto(string model, (int Min, int Max) passengerRange)
        {
            return new FlightDto
            {
                FlightStatus = Random.Shared.Next(2) == 1 ? FlightStatusDto.Arrival : FlightStatusDto.Departure,
                Model = model,
                Number = Guid.NewGuid(),
                PassengersCount = Random.Shared.Next(passengerRange.Min, passengerRange.Max)
            };
        }
    }

}
