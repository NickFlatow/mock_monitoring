using Microsoft.EntityFrameworkCore;
using mock_monitoring.Interfaces;
using mock_monitoring.Models;

namespace mock_monitoring.Repository;

public class EventRepository : IEventRepository
{
    private readonly MonitoringDbContext _dbContext;
    public EventRepository(MonitoringDbContext dbContext)
    {
        _dbContext = dbContext;
    }


    public T GetEvents<T>(Array eids) where T : Event
    {
        throw new NotImplementedException();
    }
    public async Task AddEventsAsync<T>(Event e) where T : Event
    {
        if (e == null)
        {
            throw new ArgumentNullException(nameof(e), "Event cannot be null");
        }

        _dbContext.Set<T>().Add((T)e);
        await _dbContext.SaveChangesAsync();
    }

    // public Task<List<T>> GetOpenEventsAsync<T>(int sensorId) where T : Event
    // {
    //     throw new NotImplementedException();
    // }

    public Task<List<T>> GetSensorEventsAsync<T>(int sensorId) where T : Event
    {
        throw new NotImplementedException();
    }

    Task<List<T>> IEventRepository.GetEventsAsync<T>(Array eids)
    {
        throw new NotImplementedException();
    }

    public Task<List<T>> GetOpenEventsAsync<T>(int sensorId, int evtType) where T : Event
    {
        return _dbContext.Set<T>()
            .Where(e => e.SensorId == sensorId && e.Type == evtType && e.End == 0)
            .ToListAsync();
        // throw new NotImplementedException();
    }

    public Task EscalateEventAsync<T>(Event e) where T : Event
    {
        e.EscalateEvent();
        _dbContext.Set<T>().Update((T)e);
        return _dbContext.SaveChangesAsync();
    }

    public Task CloseEventAsync<T>(Event e) where T : Event
    {
        e.Close();
        _dbContext.Set<T>().Update((T)e);
        return _dbContext.SaveChangesAsync();
    }
    
}