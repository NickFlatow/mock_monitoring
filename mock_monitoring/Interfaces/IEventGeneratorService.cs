namespace mock_monitoring.Interfaces;

using mock_monitoring.Models;


//<summary>
// Interface for the Event Generator Service which is responsible for calling all the EventGenerator classes 
// for each Event Type e.g. OutOfRangeEventGenerator, etc.
//</summary>
public interface IEventGeneratorService
{
    Task GenerateEventAsync();

}
