using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Xml.Linq;
using Lombok.NET;

namespace ScadaCore.Models;

[NoArgsConstructor]
public partial class AlarmLog {
    [Key] public int Id { get; set; }
    [Required] public int AlarmId { get; set; }
    [Required] public AlarmType Type { get; set; }
    [Required] public AlarmPriority Priority { get; set; }
    
    // TODO: Change the type of ValueName, once known
    [Required]
    [StringLength(10, MinimumLength = 1, ErrorMessage = "{0} must be between {2} and {1} characters long.")]
    public string ValueName { get; set; }
    [Required] public DateTime Timestamp { get; set; }

    public static string GetParentXElementName() {
        return "alarmLogs";
    }
    
    public static string GetXName() {
        return "alarmLog";
    }
    
    public static string GetAlarmIdXAttributeName() {
        return "alarmId";
    }
    
    public static string GetTypeXAttributeName() {
        return "type";
    }

    public static string GetPriorityXAttributeName() {
        return "priority";
    }
    
    public static string GetValueNameXAttributeName() {
        return "valueName";
    }
    
    public static string GetTimestampXAttributeName() {
        return "timestamp";
    }

    public AlarmLog(XElement alarmLogXElement) {
        Id = int.TryParse(alarmLogXElement.Value, out var value) ? value : -1;
        AlarmId =
            int.TryParse(alarmLogXElement.Attribute(GetAlarmIdXAttributeName())?.Value, out var alarmId)
                ? alarmId
                : -1;
        Type =
            Enum.TryParse(alarmLogXElement.Attribute(GetTypeXAttributeName())?.Value, out AlarmType type)
                ? type
                : default;
        Priority =
            Enum.TryParse(alarmLogXElement.Attribute(GetPriorityXAttributeName())?.Value, out AlarmPriority priority)
                ? priority
                : default;
        ValueName = alarmLogXElement.Attribute(GetValueNameXAttributeName())?.Value ?? "";
        Timestamp = DateTime.TryParseExact(
            alarmLogXElement.Attribute(GetTimestampXAttributeName())?.Value,
            "dd/MM/yyyy",
            CultureInfo.InvariantCulture,
            DateTimeStyles.None,
            out var timestamp
        ) ? timestamp : default;
    }
}