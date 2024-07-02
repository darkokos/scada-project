using Microsoft.EntityFrameworkCore;
using ScadaCore.Models;

namespace ScadaCore.Data;

public class UserContext : DbContext {
    public DbSet<User> Users => Set<User>();
    
    public UserContext(DbContextOptions<UserContext> options) : base(options) { }
}