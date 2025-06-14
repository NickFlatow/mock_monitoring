using mock_monitoring.Models;

namespace mock_monitoring.Interfaces;
public interface IEventGenerator
{
    Task CreateEvent(int sensorId);

}