

using mock_monitoring.Types;

namespace mock_monitoring.Models;

public class TemperatureSensor : Sensor
{
    private const int minTemp = 30;
    private const int maxTemp = 45;

    // todo private Profile temperatureProfile;

    public override bool IsOutOfRange(float reading)
    {
        return reading < minTemp || reading > maxTemp;
    }

    public bool IsHigh(float reading)
    {
        return reading > maxTemp;
    }
    public bool IsLow(float reading)
    {
        return reading < minTemp;
    }
    public override SensorLog addReading(float reading)
    {
        var log = new SensorLog
        {
            SensorId = this.Id,
            Temp = reading,
            Timestamp = (int)DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
            Enable = this.Enable,
            High = maxTemp, // todo get from profile
            Low = minTemp, // todo get from profile
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



