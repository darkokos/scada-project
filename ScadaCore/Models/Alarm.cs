using System.ComponentModel.DataAnnotations;

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

public class Alarm {
    [Key] public int Id { get; set; }
    [Required] public AlarmType Type { get; set; } 
    [Required] public AlarmPriority Priority { get; set; }
    
    // TODO: I am not certain about anything after this point
    
    [Required] public int Threshold { get; set; }
    
    [Required]
    [StringLength(10, MinimumLength = 1, ErrorMessage = "{0} must be between {2} and {1} characters long.")]
    public string ValueName { get; set; }
}