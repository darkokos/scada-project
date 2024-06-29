namespace Common.DatabaseManagerCommon;

public class AddAnalogInputTag
{
    public string Name { get; set; }
    public string Description { get; set; }
    public int InputOutputAddress { get; set; }
    public bool IsSimulated { get; set; }
    public TimeSpan ScanTime { get; set; }
    public bool IsScanned { get; set; }
    public HashSet<int> AlarmIds { get; set; }
    public decimal LowLimit { get; set; }
    public decimal HighLimit { get; set; }
    public string Unit { get; set; }
}