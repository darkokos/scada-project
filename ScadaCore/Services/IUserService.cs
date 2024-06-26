using ScadaCore.Models;

namespace ScadaCore.Services;

public interface IUserService {
    Task<User?> GetUserAsync(string username);
    
    Task<User?> CreateUserAsync(User user);
}