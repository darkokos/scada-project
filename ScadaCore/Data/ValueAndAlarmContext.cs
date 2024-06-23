using Microsoft.EntityFrameworkCore;
using ScadaCore.Models;

namespace ScadaCore.Data;

public class ValueAndAlarmContext : DbContext {
    public DbSet<TagLog> TagLogs => Set<TagLog>();
    public DbSet<AlarmLog> AlarmLogs => Set<AlarmLog>();

    public ValueAndAlarmContext(DbContextOptions<ValueAndAlarmContext> options) : base(options) { }
}