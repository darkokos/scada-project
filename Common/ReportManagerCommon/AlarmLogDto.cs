namespace Common.ReportManagerCommon;

public enum AlarmType {
    Low,
    High
}

public enum AlarmPriority {
    Low,
    Medium,
    High
}

public class AlarmLogDto {
    public int Id { get; set; }
    public int AlarmId { get; set; }
    public AlarmType Type { get; set; }
    public AlarmPriority Priority { get; set; }
    public string Unit { get; set; }
    public DateTime Timestamp { get; set; }
    
}