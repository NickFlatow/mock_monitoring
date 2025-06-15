using mock_monitoring.Models;
using mock_monitoring.Repository;
using mock_monitoring.Interfaces;

namespace mock_monitoring.Mock_Sensor;

public class SensorDataGeneratorService(IServiceProvider serviceProvider) : BackgroundService
{
    private readonly IServiceProvider _serviceProvider = serviceProvider;
    private readonly TimeSpan _interval = TimeSpan.FromSeconds(60);

    private ISensorRepository? _sensorRepository;
    private readonly Random _random = new Random();

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            await GenerateMockSensorData(stoppingToken);
            await Task.Delay(_interval, stoppingToken);
        }
    }

    private async Task GenerateMockSensorData(CancellationToken stoppingToken)
    {

        using var scope = _serviceProvider.CreateScope();
        _sensorRepository = scope.ServiceProvider.GetRequiredService<ISensorRepository>();
        // generate between 20 and 55 
        await _sensorRepository.AddReadingAsync<TemperatureSensor>(1, (float)(_random.NextDouble() * 20 + 35));
    }
}