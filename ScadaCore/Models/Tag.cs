using System.ComponentModel.DataAnnotations;

namespace ScadaCore.Models;

public class Tag {
    [Key] public string Name { get; set; }
    
    [StringLength(250, MinimumLength = 0, ErrorMessage = "{0} should be between {2} and {1} characters long.")]
    public string Description { get; set; }
    
    // TODO: Change the type of this when known
    [Required] public int InputOutputAddress { get; set; }
}