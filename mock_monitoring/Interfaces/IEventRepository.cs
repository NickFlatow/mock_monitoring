namespace mock_monitoring.Interfaces;
using mock_monitoring.Models;

public interface IEventRepository
{

    Task<List<T>> GetOpenEventsAsync<T>(int sensorId) where T : Event;

    Task<List<T>> GetSensorEventsAsync<T>(int sensorId) where T : Event;
    Task<List<T>> GetEventsAsync<T>(Array eids) where T : Event;
    Task AddEventsAsync<T>(Event e) where T : Event;
    
}
