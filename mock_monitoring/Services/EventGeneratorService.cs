namespace mock_monitoring.Services;

using mock_monitoring.Models;
using mock_monitoring.Interfaces;
using mock_monitoring.Repository;
using mock_monitoring.Lib.EventGenerators;
using System;

public class EventGeneratorService(IServiceProvider serviceProvider) : BackgroundService, IEventGeneratorService
{

    private readonly IServiceProvider _serviceProvider = serviceProvider;
    private  ISensorRepository? _sensorRepository;
    // private readonly ISensorService _sensorService;

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
        IEventRepository eventRepository = scope.ServiceProvider.GetRequiredService<IEventRepository>();


        // Retrieve all sensors of type T from the repository
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
            var oor = new OutOfRangeEventGenerator(eventRepository, _sensorRepository);
            await oor.CreateEvent(sensor.Id);





        }
        // Retrieve the sensor from the repository
        // var sensor = await _sensorRepository.GetSensorAsync<T>(sensorId);
        // if (sensor == null)
        // {
        //     throw new KeyNotFoundException($"Sensor with ID {sensorId} not found.");
        // }
        // Console.WriteLine($"Sensor {sensor.Name} with ID {sensorId} has value {value}.");

        // return false;
    }
}