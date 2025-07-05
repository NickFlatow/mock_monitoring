using mock_monitoring.Models;

namespace mock_monitoring.Interfaces;

public interface IEventGenerator
{
    //proccess current stat of the sensor
    Task Process(int sensorId);

    //if an event already exists, process it
    //this is used to update the event status or close it
    Task ProcessExsistingEvent(Event evt);

    //genareate an event based on the sensor state
    Task CreateEvent(int sensorId);
    


}