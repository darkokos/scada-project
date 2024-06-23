namespace Common.DatabaseManagerCommon
{
    public enum AlarmType {
        Low,
        High
    }

    public enum AlarmPriority {
        Low,
        Medium,
        High
    }
    
    public class AddAlarmDTO
    {
        public string token { get; set; }
        public AlarmType Type { get; set; } 
        public AlarmPriority Priority { get; set; }
        public int Threshold { get; set; }
        public string ValueName { get; set; }
    }
}