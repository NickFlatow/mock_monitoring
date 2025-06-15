using mock_monitoring.Lib.Events;
using mock_monitoring.Interfaces;
using mock_monitoring.Models;
using mock_monitoring.Types;

namespace mock_monitoring.Lib.EventGenerators;

public class NetworkEventGenerator : IEventGenerator
{

    private readonly IEventRepository _eventRepository;
    private readonly ISensorRepository _sensorRepository;
     
    private readonly int delayTolerance = 61;

    public NetworkEventGenerator(IEventRepository eventRepository, ISensorRepository sensorRepository)
    {
        _eventRepository = eventRepository;
        _sensorRepository = sensorRepository;
    }
    public async Task CreateEvent(int sensorId)
    {
        var sensor = await _sensorRepository.GetSensorAsync<Sensor>(sensorId);
        if (sensor == null)
        {
            Console.WriteLine($"Sensor with ID {sensorId} not found.");
            return;
        }
        // Check if the sensor is enabled
        if (!sensor.Enable)
        {
            Console.WriteLine($"Sensor {sensor.Name} with ID {sensor.Id} is disabled.");
            return;
        }
        var sensorLog = await _sensorRepository.GetLatestSensorLogAsync(sensorId);
        if (sensorLog == null)
        {
            Console.WriteLine($"No sensor log found for sensor ID {sensorId}");
            return;
        }
        var now = (int)DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        var expectedTimestamp = now - sensor.Sample_Period;
        var actualTimestamp = sensorLog.Timestamp + delayTolerance;

        if (expectedTimestamp > actualTimestamp)
        {
            // If the current time plus the sample period is less than the last log timestamp plus the delay tolerance,
            // it indicates a network event.
            var networkEvent = new NetworkEvent
            {
                SensorId = sensorLog.SensorId,
                SensorLogId = sensorLog.Id,
                Quality = 0,
                Status = sensorLog.Status,
                Type = EventTypes.NetworkEvent,
                Level_1 = 0,
                Level_2 = 0,
                Level_3 = 0,
                Start = (int)DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
                End = 0,
            };
            await _eventRepository.AddEventsAsync<Event>(networkEvent);
        }
    }
}