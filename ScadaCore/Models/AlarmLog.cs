using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ScadaCore.Models;

public class AlarmLog {
    [Key] public int Id { get; set; }
    
    // TODO: Either like this, if reference to alarm is needed for some behaviour, or just track the logged message, to
    // avoid a join in the db
    [Required] [ForeignKey("Alarm")] public int AlarmId { get; set; }
    public virtual Alarm Alarm { get; set; }
    
    [Required] public DateTime Timestamp { get; set; }
}