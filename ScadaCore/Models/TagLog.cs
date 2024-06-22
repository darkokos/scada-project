using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ScadaCore.Models;

public class TagLog {
    [Key] public int Id { get; set; }
    
    // TODO: Either like this, if reference to alarm is needed for some behaviour, or just track the logged message, to
    // avoid a join in the db
    [Required] [ForeignKey("Tag")] public string TagName { get; set; }
    public virtual Tag Tag { get; set; }
    
    [Required] public DateTime Timestamp { get; set; }
}