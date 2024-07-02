namespace Common.DatabaseManagerCommon;

public class AddDigitalInputTag
{
    public string Name { get; set; }
    public string Description { get; set; }
    public int InputOutputAddress { get; set; }
    public bool IsSimulated { get; set; }
    public TimeSpan ScanTime { get; set; }
    public bool IsScanned { get; set; }
}
