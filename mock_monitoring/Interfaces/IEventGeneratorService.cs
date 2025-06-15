namespace mock_monitoring.Interfaces;

using mock_monitoring.Models;
using mock_monitoring.Interfaces;


//<summary>
// Interface for the Event Generator Service which is responsible for calling all the EventGenerator classes 
// for each Event Type e.g. OutOfRangeEventGenerator, etc.
//</summary>
public interface IEventGeneratorService
{
    Task GenerateEventAsync();
    List<IEventGenerator> GetEventGenerators<T>() where T : IEventGenerator;

}
