
namespace mock_monitoring.Types;

public class EventStatus
{
    public const int Normal = 0;
    public const int Low = 1;
    public const int High = 2;

    public static string GetStatusDescription(int status)
    {
        return status switch
        {
            Normal => "Normal",
            Low => "Low",
            High => "High",
            _ => "Unknown"
        };
    }

    public static string getTableCSSClass(int status)
    {
        return status switch
            {
                Normal => "table-success",
                Low => "table-primary",
                High => "table-danger",
                _ => "table-secondary"
            };

    }

}