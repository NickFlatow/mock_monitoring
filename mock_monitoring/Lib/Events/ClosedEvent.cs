using mock_monitoring.Models;
using mock_monitoring.Types;
namespace mock_monitoring.Lib.Events;

public class ClosedEvent : Event
{


    public override void Close()
    {
        throw new InvalidOperationException("Closed events cannot be closed again.");
    }

    //Base Event??
    public override void EscalateEvent()
    {
        // Closed events do not escalate
        // This method can be left empty or throw an exception if escalation is not allowed
        throw new InvalidOperationException("Closed events cannot be escalated.");
    }
}