using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace ScadaCore.Models;

[Index(nameof(Username), IsUnique = true)]
public class User {
    [Key] public int Id { get; set; }
    
    [Required]
    [StringLength(254, MinimumLength = 6, ErrorMessage = "{0} should be between {2} and {1} characters long.")]
    public string Username { get; set; }
    
    [Required]
    [StringLength(int.MaxValue, MinimumLength = 7, ErrorMessage = "{0} should be between {2} and {1} characters long.")]
    public string Password { get; set; }
}
