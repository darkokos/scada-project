using System.ComponentModel.DataAnnotations;

namespace ScadaCore.Models;

public class DigitalTagLog : TagLog {
    [Required] public bool EmittedValue { get; set; }
}