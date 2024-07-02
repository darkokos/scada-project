using Microsoft.EntityFrameworkCore;
using ScadaCore.Models;

namespace ScadaCore.Data;

public class ValueAndAlarmContext : DbContext {
    public DbSet<TagLog> TagLogs => Set<TagLog>();
    public DbSet<AnalogTagLog> AnalogTagLogs => Set<AnalogTagLog>();
    public DbSet<DigitalTagLog> DigitalTagLogs => Set<DigitalTagLog>();
    public DbSet<AlarmLog> AlarmLogs => Set<AlarmLog>();

    public ValueAndAlarmContext(DbContextOptions<ValueAndAlarmContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder) {
        modelBuilder.Entity<TagLog>().UseTptMappingStrategy();
    }
}