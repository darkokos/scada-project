using System.ComponentModel.DataAnnotations;

namespace ScadaCore.Models;

public class AnalogTagLog : TagLog{
    [Required] public decimal EmittedValue { get; set; }
}