using mock_monitoring.Lib.Events;
using mock_monitoring.Interfaces;
using mock_monitoring.Models;
using mock_monitoring.Types;
using Castle.Core.Logging;

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

    public async Task Process(int sensorId)
    {
        var events = await _eventRepository.GetOpenEventsAsync<OutOfRangeEvent>(sensorId, EventTypes.OutOfRange);
        if (events != null && events.Count > 0)
        {

            foreach (var evt in events)
            {
                // elevate, close
                await ProcessExsistingEvent(evt);
            }
        }
        await CreateEvent(sensorId);

    }
    public async Task ProcessExsistingEvent(Event evt)
    {

        //first check if the event is back in range if so close it
        var sensorLog = await _sensorRepository.GetLatestSensorLogAsync(evt.SensorId);
        if (sensorLog != null && sensorLog.Quality == Quality.Good && sensorLog.Status == EventStatus.Normal)
        {
            await _eventRepository.CloseEventAsync<OutOfRangeEvent>(evt);
            return;
        }

        //What if we switch from low to high event?

        //todo add profile logic for escalation times
        var level1 = 300;
        var level2 = 600;
        var level3 = 900;
        var now = (int)DateTimeOffset.UtcNow.ToUnixTimeSeconds();

        var currentLevel = evt.Current_Level;
        switch (currentLevel)
        {
            case EventAlarmLevels.Level0:
                if (now - evt.Start > level1)
                {
                   await _eventRepository.EscalateEventAsync<OutOfRangeEvent>(evt);

                }
                break;
            case EventAlarmLevels.Level1:
                if (now - evt.Start > level2)
                {
                   await _eventRepository.EscalateEventAsync<OutOfRangeEvent>(evt);
                }
                break;
            case EventAlarmLevels.Level2:
                if (now - evt.Start > level3)
                {
                    // idk here
                    await _eventRepository.EscalateEventAsync<OutOfRangeEvent>(evt);
                }
                break;
        }

    }
    public async Task CreateEvent(int sensorId)
    {

        //only one oor event per sensor
        var events = await _eventRepository.GetOpenEventsAsync<OutOfRangeEvent>(sensorId, EventTypes.OutOfRange);
        if (events != null && events.Count > 0)
        {
            Console.WriteLine($"OutOfRangeEvent already exists for sensor ID {sensorId}");
            return;
        }


        var sensorLog = await _sensorRepository.GetLatestSensorLogAsync(sensorId);
        if (sensorLog == null)
        {
            Console.WriteLine($"No sensor log found for sensor ID {sensorId}");
            return;
        }

        else if (sensorLog.Status != EventStatus.Normal)
        {
            var oorEvent = new OutOfRangeEvent
            {
                SensorId = sensorLog.SensorId,
                SensorLogId = sensorLog.Id,
                Quality = 0,
                Status = sensorLog.Status,
                Type = EventTypes.OutOfRange,
                Current_Level = EventAlarmLevels.Level0,
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