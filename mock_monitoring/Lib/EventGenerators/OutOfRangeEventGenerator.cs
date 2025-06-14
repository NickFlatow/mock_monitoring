using mock_monitoring.Lib.Events;
using mock_monitoring.Interfaces;
using mock_monitoring.Models;
using mock_monitoring.Types;

namespace mock_monitoring.Lib.EventGenerators;

public class OutOfRangeEventGenerator : IEventGenerator
{

    private readonly IEventRepository _eventRepository;
    private readonly ISensorRepository _sensorRepository;
    public OutOfRangeEventGenerator(IEventRepository eventRepository, ISensorRepository sensorRepository)
    {
        _eventRepository = eventRepository;
        _sensorRepository = sensorRepository;
    }
    public async Task CreateEvent(int sensorId)
    {
        var sensorLog = await _sensorRepository.GetLatestSensorLogAsync(sensorId);
        if (sensorLog == null)
        {
            Console.WriteLine($"No sensor log found for sensor ID {sensorId}");
            return;
        }

        else if (sensorLog.Status != Status.Normal)
        {
            var oorEvent = new OutOfRangeEvent
            {
                SensorId = sensorLog.SensorId,
                SensorLogId = sensorLog.Id,
                Quality = 0,
                Status = sensorLog.Status,
                Type = EventTypes.OutOfRange,
                Level_1 = 0,
                Level_2 = 0,
                Level_3 = 0,
                Start = (int)DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
                End = 0,
            };
            await _eventRepository.AddEventsAsync<Event>(oorEvent);
        }

    }
}