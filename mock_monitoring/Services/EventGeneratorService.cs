namespace mock_monitoring.Services;

using mock_monitoring.Models;
using mock_monitoring.Interfaces;
using mock_monitoring.Repository;
using mock_monitoring.Lib.EventGenerators;
using System;

public class EventGeneratorService(IServiceProvider serviceProvider) : BackgroundService, IEventGeneratorService
{

    private readonly IServiceProvider _serviceProvider = serviceProvider;
    private ISensorRepository? _sensorRepository;
    private IEventRepository? _eventRepository;
    private readonly TimeSpan _interval = TimeSpan.FromSeconds(60);

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            // await GenerateEventAsync<Sensor>(1, 25.0); 
            await GenerateEventAsync();
            await Task.Delay(_interval, stoppingToken);
        }
    }



    public async Task GenerateEventAsync()
    {
        using var scope = _serviceProvider.CreateScope();

        _sensorRepository = scope.ServiceProvider.GetRequiredService<ISensorRepository>();
        _eventRepository = scope.ServiceProvider.GetRequiredService<IEventRepository>();
        var sensors = await _sensorRepository.GetAllSensorsAsync<Sensor>();
        foreach (var sensor in sensors)
        {
            // Check if the sensor ID matches the provided sensorId

            // Check if the sensor is enabled
            if (!sensor.Enable)
            {
                Console.WriteLine($"Sensor {sensor.Name} with ID {sensor.Id} is disabled.");
                continue;
            }
            
            var eventGenerators = GetEventGenerators<IEventGenerator>();
            foreach (var eventGenerator in eventGenerators)
            {
                // Call the CreateEvent method of the event generator
                await eventGenerator.CreateEvent(sensor.Id);
            }

        }
    }
    public List<IEventGenerator> GetEventGenerators<T>() where T : IEventGenerator
    {
        if (_eventRepository == null || _sensorRepository == null)
        {
            throw new InvalidOperationException("Repositories must be initialized before creating event generators.");
        }

        var eventGenerators = new List<IEventGenerator>
        {
            new OutOfRangeEventGenerator(_eventRepository, _sensorRepository)
        };

        return eventGenerators;
    }
}