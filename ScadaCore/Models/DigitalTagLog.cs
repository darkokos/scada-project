using System.ComponentModel.DataAnnotations;

namespace ScadaCore.Models;

public class DigitalTagLog : TagLog {
    [Required] public bool EmittedValue { get; set; }
    
    public DigitalTagLog() { }

    public DigitalTagLog(string tagName, DateTime timestamp, bool emittedValue) : base(tagName, timestamp) {
        EmittedValue = emittedValue;
    }
}