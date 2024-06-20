namespace ScadaCore.Models;

public class AnalogInputTag : InputTag {
    public ICollection<Alarm> Alarms { get; set; }
    
    // TODO: Again, no idea what type these are supposed to be
    
    public int LowLimit { get; set; }
    public int HighLimit { get; set; }
    
    // TODO: Units??!
    
    public HashSet<string> Units { get; set; }
}