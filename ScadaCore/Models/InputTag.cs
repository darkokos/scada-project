using System.ComponentModel.DataAnnotations;

namespace ScadaCore.Models;

public class InputTag : Tag {
    
    // TODO: Whatever type this is supposed to be
    [Required] public string Driver { get; set; }

    // TODO: This is supposed to represent a period. I don't know whether there is a better type to represent that, or
    // whether it will be necessary at all
    [Required] public int ScanTime { get; set; }
    [Required] public bool IsScanned { get; set; }
}