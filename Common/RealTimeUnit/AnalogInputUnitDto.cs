namespace Common.RealTimeUnit;

public class AnalogInputUnitDto {
    public string TagName { get; set; }
    public TimeSpan ScanTime { get; set; }
    public decimal LowLimit { get; set; }
    public decimal HighLimit { get; set; }
}