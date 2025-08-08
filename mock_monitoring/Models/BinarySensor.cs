

using mock_monitoring.Types;

namespace mock_monitoring.Models;

public class BinarySensor : Sensor
{
    //todo min/max temps in C conversion to F on the template 
    private const int HIGH = 1;
    private const int LOW = 0;

    // todo private Profile temperatureProfile;

    public override bool IsOutOfRange(float reading)
    {
        return true;
        // return reading < minTemp || reading > maxTemp;
    }

    public bool IsHigh(float reading)
    {
        return reading == HIGH;
        // return reading > maxTemp;
    }
    public bool IsLow(float reading)
    {
        return reading == LOW;
        // return reading < minTemp;
        // return reading < minTemp;
    }
    public override SensorLog addReading(float reading)
    {

        var log = new SensorLog
        {
            SensorId = this.Id,
            Temp = reading,
            Timestamp = (int)DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
            Enable = this.Enable,
            High = HIGH, // todo get from profile
            Low = LOW, // todo get from profile
            Status = getStatus(reading),
            Quality = getQuality(reading)

        };

        return log;
    }
    public override int getStatus(float reading)
    {
        if (IsHigh(reading))
        {
            return EventStatus.High;
        }
        else if (IsLow(reading))
        {
            return EventStatus.Low;
        }
        return EventStatus.Normal;
    }
    public override int getQuality(float reading)
    {
        return Quality.Good;
    }


}



