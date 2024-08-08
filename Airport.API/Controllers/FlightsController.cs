using Microsoft.AspNetCore.Mvc;
using Airport.API.Models;
using Airport.API.Repositories;
using Airport.API.Services.AirportService;

namespace Airport.API.Controllers
{
    [ApiController]
    public class FlightsController(IAirportRepository repository, IAirportService airportService) : ControllerBase
    {
        private readonly IAirportRepository _repository = repository;
        private readonly IAirportService _airportService = airportService;

        [Route("/api/post/flight")]
        [HttpPost]
        public async Task<IActionResult> PostFlight(Flight flight)
        {
            await _airportService.AddFlightAsync(flight);
            return StatusCode(201, flight);
        }
        [HttpGet]
        [Route("/api/get/logs")]
        public async Task<IActionResult> GetLogs(int lastLogId)
        {
            return Ok(await _repository.GetLogsAsync(lastLogId));
        }
        [HttpGet]
        [Route("/api/get/legs")]
        public async Task<IActionResult> GetLegs()
        {
            return Ok(await _repository.GetLegsAsync());
        }
        [HttpGet]
        [Route("/api/get/flights")]
        public async Task<IActionResult> GetFlights()
        {
            var flights = await _repository.GetFlightsAsync();
            return Ok(flights);
        }
        [HttpPut]
        [Route("/api/complete/moving/flights")]
        public async Task<IActionResult> CompleteMovingFlights()
        {
            await _repository.CompleteMovingFlights();
            return Ok();
        }
        [HttpDelete]
        [Route("/api/reset/database")]
        public async Task<IActionResult> ResetDatabase()
        {
            await _repository.ResetDatabase();
            return Ok();
        }
    }
}
