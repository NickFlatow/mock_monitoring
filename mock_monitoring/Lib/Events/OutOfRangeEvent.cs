using mock_monitoring.Models;
using mock_monitoring.Types;

namespace mock_monitoring.Lib.Events;

public class OutOfRangeEvent : Event
{



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