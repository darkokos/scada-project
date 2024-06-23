using System.ComponentModel.DataAnnotations;

namespace ScadaCore.Models;

public class AlarmLog {
    [Key] public int Id { get; set; }
    [Required] public int AlarmId { get; set; }
    [Required] public AlarmType Type { get; set; }
    
    // TODO: Change the type of ValueName, once known
    [Required]
    [StringLength(10, MinimumLength = 1, ErrorMessage = "{0} must be between {2} and {1} characters long.")]
    public string ValueName { get; set; }
    [Required] public DateTime Timestamp { get; set; }
}