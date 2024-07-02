using ScadaCore.Models;

namespace ScadaCore.Repositories;

public interface IUserRepository {
    Task<User?> GetUserAsync(string username);
    
    Task<User?> CreateUserAsync(User user);
}