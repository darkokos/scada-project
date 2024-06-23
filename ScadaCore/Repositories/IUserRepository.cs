using ScadaCore.Models;

namespace ScadaCore.Repositories;

public interface IUserRepository {
    Task<User?> GetUserByUsernameAsync(string username);
    
    Task<User?> CreateUserAsync(User user);
}