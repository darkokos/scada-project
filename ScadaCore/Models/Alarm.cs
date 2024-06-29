using System.Xml.Linq;
using Lombok.NET;

namespace ScadaCore.Models;

public enum AlarmType {
    Low,
    High
}

public enum AlarmPriority {
    Low,
    Medium,
    High
}

[NoArgsConstructor]
public partial class Alarm {
    public int Id { get; set; }
    public AlarmType Type { get; set; } 
    public AlarmPriority Priority { get; set; }
    public decimal Threshold { get; set; }
    public string Unit { get; set; }

    public static string GetParentXElementName() {
        return "alarms";
    }

    public static string GetXName() {
        return "alarm";
    }
    
    public static string GetTypeXAttributeName() {
        return "type";
    }
    
    public static string GetPriorityXAttributeName() {
        return "priority";
    }
    
    public static string GetThresholdXAttributeName() {
        return "threshold";
    }
    
    public static string GetUnitXAttributeName() {
        return "unit";
    }

    public Alarm(XElement alarmXElement) {
        Id = int.TryParse(alarmXElement.Value, out var id) ? id : -1;
        Type =
            Enum.TryParse(alarmXElement.Attribute(GetTypeXAttributeName())?.Value, out AlarmType alarmType)
                ? alarmType
                : default;
        Priority = Enum.TryParse(
            alarmXElement.Attribute(GetPriorityXAttributeName())?.Value,
            out AlarmPriority alarmPriority
        ) ? alarmPriority : default;
        Threshold =
            decimal.TryParse(alarmXElement.Attribute(GetThresholdXAttributeName())?.Value, out var threshold)
                ? threshold
                : -1;
        Unit = alarmXElement.Attribute(GetUnitXAttributeName())?.Value ?? "";
    }

    public Alarm(AlarmType type, AlarmPriority priority, decimal threshold, string unit)
    {
        Type = type;
        Priority = priority;
        Threshold = threshold;
        Unit = unit;
    }
}