namespace Common.RealTimeUnit;

public class AnalogTagLogDto {
    public int Id { get; set; }
    public string TagName { get; set; }
    public decimal EmittedValue { get; set; }
    public DateTime Timestamp { get; set; }
}