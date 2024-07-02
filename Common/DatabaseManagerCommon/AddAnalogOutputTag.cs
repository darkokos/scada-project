namespace Common.DatabaseManagerCommon;

public class AddAnalogOutputTag
{
    public string Name { get; set; }
    public string Description { get; set; }
    public int InputOutputAddress { get; set; }
    public decimal InitialValue { get; set; }
    public decimal LowLimit { get; set; }
    public decimal HighLimit { get; set; }
    public string Unit { get; set; }
}