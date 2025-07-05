
using mock_monitoring.Models;
using mock_monitoring.Types;
namespace mock_monitoring.Lib.Events;

public class OutOfRangeEvent : Event
{


    public override void Close()
    {
        End = (int)DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        Status = EventStatus.Normal; // Set status to normal when closing the event
        // Type = EventTypes.Closed; // Set type to closed when closing the event
        Current_Level = EventAlarmLevels.Level0; // Reset to normal state
    }

    //Base Event??
    public override void EscalateEvent()
    {
        var now = (int)DateTimeOffset.UtcNow.ToUnixTimeSeconds();

        if (Current_Level == EventAlarmLevels.Level0)
        {
            Current_Level = EventAlarmLevels.Level1;
            Level_1 = now;
        }
        if (Current_Level == EventAlarmLevels.Level1)
        {
            Current_Level = EventAlarmLevels.Level2;
            Level_2 = now;
        }
        else if (Current_Level == EventAlarmLevels.Level2)
        {
            Current_Level = EventAlarmLevels.Level3;
            Level_3 = now;
        }
        // else if (Current_Level == EventAlarmLevels.Level3)
        // {
        //     // Already at maximum escalation level
        // }
    }


    // public override void CreateEvent(SensorLog sensorLog)
    // {
    //     SensorId = sensorLog.SensorId;
    //     SensorLogId = sensorLog.Id;
    //     Quality = 0;
    //     Status = sensorLog.Status;
    //     Type = EventTypes.OutOfRange;
    // }


    // public override void CreateEvent(SensorLog sensorLog)
    // {
    //     SensorId = sensorLog.SensorId;
    //     SensorLogId = sensorLog.Id;
    //     Quality = 0;
    //     Status = sensorLog.Status;
    //     Type = EventTypes.OutOfRange;
    //     Level_1 = 0;
    //     Level_2 = 0;
    //     Level_3 = 0;
    //     Start = (int)DateTimeOffset.UtcNow.ToUnixTimeSeconds();
    //     End = 0;
    // }

}