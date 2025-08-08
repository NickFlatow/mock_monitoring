using mock_monitoring.Interfaces;


namespace mock_monitoring.Lib.Sensors.esp32;
public class TempHumidity : IEsp32
{
    public string MAC { get; set; }
    public string TYPE { get; set; } 
    public float? SD1 { get; set; } // Temperature
    public float? SD2 { get; set; } // Humidity

    // Additional properties can be added as needed
}