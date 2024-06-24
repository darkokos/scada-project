using System.ComponentModel.DataAnnotations;
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
    [Required] public AlarmType Type { get; set; } 
    [Required] public AlarmPriority Priority { get; set; }
    
    // TODO: I am not certain about anything after this point
    [Required] public int Threshold { get; set; }
    
    [Required]
    [StringLength(10, MinimumLength = 1, ErrorMessage = "{0} must be between {2} and {1} characters long.")]
    public string ValueName { get; set; }

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
    
    public static string GetValueNameXAttributeName() {
        return "valueName";
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
            int.TryParse(alarmXElement.Attribute(GetThresholdXAttributeName())?.Value, out var threshold)
                ? threshold
                : -1;
        ValueName = alarmXElement.Attribute(GetValueNameXAttributeName())?.Value ?? "";
    }
}