using Airport.API.Data;
using Airport.API.Repositories;
using Airport.API.Services.AirportService;
using Airport.API.Services.FlightMovement;
using Airport.API.Services.LegService;
using Airport.API.Services.WaitingFlightsProcessing;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

namespace Airport.API
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddDbContext<AirportContext>(options =>
            {
                options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"));
            });
            builder.Services.AddControllers().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
            }); 
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", options =>
                {
                    options.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
                });
            });
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddSingleton<IFlightMovementService, FlightMovementService>();
            builder.Services.AddSingleton<ILegService, LegService>();
            builder.Services.AddSingleton<IAirportService, AirportService>();
            builder.Services.AddScoped<IAirportRepository, AirportRepository>();
            builder.Services.AddHostedService<WaitingFlightProcessingHostedService>();

            var app = builder.Build();

            using (var scope = app.Services.CreateScope())
            {
                var repository = scope.ServiceProvider.GetRequiredService<IAirportRepository>();
                if (!repository.EnsureMigrated())
                {
                    await repository.CompleteMovingFlights();
                }
            }

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseAuthorization();

            app.UseCors("AllowAll");

            app.MapControllers();

            app.Run();
        }
    }
}
