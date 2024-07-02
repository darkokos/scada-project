using System.ComponentModel.DataAnnotations;

namespace ScadaCore.Models;

public class AnalogTagLog : TagLog {
    [Required] public decimal EmittedValue { get; set; }
    
    public AnalogTagLog() { }

    public AnalogTagLog(string tagName, DateTime timestamp, decimal emittedValue) : base(tagName, timestamp) {
        EmittedValue = emittedValue;
    }
}