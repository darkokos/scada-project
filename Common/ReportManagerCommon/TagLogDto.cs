namespace Common.ReportManagerCommon;

public class TagLogDto {
    public int Id { get; set; }
    public string TagName { get; set; }
    public DateTime Timestamp { get; set; }
    public decimal EmittedValue { get; set; }
}
