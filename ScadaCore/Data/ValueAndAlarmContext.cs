using Microsoft.EntityFrameworkCore;
using ScadaCore.Models;

namespace ScadaCore.Data;

public class ValueAndAlarmContext : DbContext {
    public DbSet<TagLog> TagLogs => Set<TagLog>();
    public DbSet<AlarmLog> AlarmLogs => Set<AlarmLog>();

    public ValueAndAlarmContext(DbContextOptions<ValueAndAlarmContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder) {
        modelBuilder.Entity<InputTag>().ToTable("InputTags");
        modelBuilder.Entity<OutputTag>().ToTable("OutputTags");
        modelBuilder.Entity<DigitalInputTag>().ToTable("DigitalInputTags");
        modelBuilder.Entity<DigitalOutputTag>().ToTable("DigitalOutputTags");
        modelBuilder.Entity<AnalogInputTag>().ToTable("AnalogInputTags");
        modelBuilder.Entity<AnalogOutputTag>().ToTable("AnalogOutputTags");
    }
}