using System;

namespace mock_monitoring.Interfaces
{
    public interface ISensorData
    {
        string PASS { get; set; }
        string MAC { get; set; }
        string TYPE { get; set; }
        float? SD1 { get; set; }
        float? SD2 { get; set; }
        int BATT { get; set; }
        DateTime TIME { get; set; }
        int TSEL { get; set; }
        string UNIT1 { get; set; }
        string UNIT2 { get; set; }
        int BITRATE { get; set; }
        int POWER { get; set; }
        int INFO { get; set; }
        int OCstatus { get; set; }
    }
}