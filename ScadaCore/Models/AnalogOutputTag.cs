namespace ScadaCore.Models;

public class AnalogOutputTag : OutputTag {
    
    // TODO: Types
    
    public int LowLimit { get; set; }
    public int HighLimit { get; set; }
    
    // TODO: Units
    
    public HashSet<string> Units { get; set; }
}
