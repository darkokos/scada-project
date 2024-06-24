using ScadaCore.Data;
using ScadaCore.Models;

namespace ScadaCore.Repositories;

public class UserRepository(UserContext userContext) : IUserRepository {
    public async Task<User?> GetUserAsync(string username) {
        return await userContext.Users.FindAsync(username);
    }

    public async Task<User?> CreateUserAsync(User user) {
        var savedUser = await userContext.Users.AddAsync(user);
        return await userContext.SaveChangesAsync() >= 0 ? savedUser.Entity : null;
    }
}