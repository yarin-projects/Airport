using Airport.API.Services.AirportService;

namespace Airport.API.Services.WaitingFlightsProcessing
{
    public class WaitingFlightProcessingHostedService(IServiceScopeFactory serviceScopeFactory) : IHostedService, IDisposable
    {
        private readonly IServiceScopeFactory _serviceScopeFactory = serviceScopeFactory;
        private Timer _timer;
        private bool _disposed = false;
        private readonly TimeSpan startTimerIn = TimeSpan.FromSeconds(5);
        private readonly TimeSpan timerInterval = TimeSpan.FromSeconds(3);

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _timer = new Timer(async (state) => await ProcessWaitingFlights(), null, startTimerIn, timerInterval);
            return Task.CompletedTask;
        }
        private async Task ProcessWaitingFlights()
        {
            using var scope = _serviceScopeFactory.CreateScope();
            var airportService = scope.ServiceProvider.GetRequiredService<IAirportService>();
            await airportService.ProcessWaitingFlights();
        }
        public Task StopAsync(CancellationToken cancellationToken)
        {
            _timer?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _timer?.Dispose();
                }
                _disposed = true;
            }
        }
    }
}
