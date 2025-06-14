namespace mock_monitoring.Interfaces;
using mock_monitoring.Models;

public interface ISensorRepository
{
    Task<List<T>> GetAllSensorsAsync<T>() where T : Sensor;
    //todo update sensorId to macAddress
    Task<T> GetSensorAsync<T>(int sensorId) where T : Sensor;
    Task AddReadingAsync<T>(int sensorId, float reading) where T : Sensor;

    Task<SensorLog> GetLatestSensorLogAsync(int sensorId);
    // Task<List<SensorLog>> GetSensorLogsAsync(int sensorId, DateTime startDate, DateTime endDate);

}
