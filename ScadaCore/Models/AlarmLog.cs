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
    
    // TODO: Change the type of ValueName, once known
    [Required]
    [StringLength(10, MinimumLength = 1, ErrorMessage = "{0} must be between {2} and {1} characters long.")]
    public string ValueName { get; set; }
    [Required] public DateTime Timestamp { get; set; }

    public AlarmLog(XElement alarmLogXElement) {
        Id = int.TryParse(alarmLogXElement.Value, out var value) ? value : -1;
        AlarmId = int.TryParse(alarmLogXElement.Attribute("alarmId")?.Value, out var alarmId) ? alarmId : -1;
        Type = Enum.TryParse(alarmLogXElement.Attribute("type")?.Value, out AlarmType type) ? type : AlarmType.High;
        ValueName = alarmLogXElement.Attribute("valueName")?.Value ?? "";
        Timestamp =
            DateTime
                .TryParseExact(
                    alarmLogXElement.Attribute("timestamp")?.Value,
                    "dd/MM/yyyy",
                    CultureInfo.InvariantCulture,
                    DateTimeStyles.None,
                    out var timestamp
                )
                ? timestamp
                : default;
    }
}