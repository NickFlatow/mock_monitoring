namespace mock_monitoring.Repository;


using mock_monitoring.Models;
using mock_monitoring.Interfaces;
using mock_monitoring.Types;
using Microsoft.EntityFrameworkCore;


public class SensorRepository : ISensorRepository
{
    private readonly MonitoringDbContext _dbContext;
    public SensorRepository(MonitoringDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<T> GetSensorAsync<T>(int sensorId) where T : Sensor
    {
        var sensor = await _dbContext.Set<T>().FirstOrDefaultAsync(s => s.Id == sensorId);

        if (sensor == null)
        {
            throw new KeyNotFoundException($"Sensor with ID {sensorId} not found.");
        }
        return sensor;
    }

    public async Task AddReadingAsync<T>(int sensorId, float reading) where T : Sensor
    {
        var sensor = await GetSensorAsync<T>(sensorId);
        var log = sensor.addReading(reading);

        // if (log.Status != Status.Normal)
        // {
        //     // Log the event if the status is not normal
        //     var eventRepository = new EventRepository(_dbContext);
        //     await eventRepository.AddEventsAsync(new BaseEvent
        //     {
        //         SensorId = sensorId,
        //         Type = log.Status,
        //         Timestamp = log.Timestamp,
        //         Description = $"Sensor {sensor.Name} reading out of range: {reading}"
        //     });
        // }

        _dbContext.SensorLog.Add(log);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<List<T>> GetAllSensorsAsync<T>() where T : Sensor
    {
        return await _dbContext.Set<T>().ToListAsync();
    }
    
    public async Task<SensorLog?> GetLatestSensorLogAsync(int sensorId)
    {
        return await _dbContext.SensorLog
            .Where(log => log.SensorId == sensorId)
            .OrderByDescending(log => log.Timestamp)
            .FirstOrDefaultAsync();
    }

}