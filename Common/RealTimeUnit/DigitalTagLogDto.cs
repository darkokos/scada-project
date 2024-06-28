namespace Common.RealTimeUnit;

public class DigitalTagLogDto {
    public int Id { get; set; }
    public string TagName { get; set; }
    public bool EmittedValue { get; set; }
    public DateTime Timestamp { get; set; }
}