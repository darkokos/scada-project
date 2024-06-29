using ScadaCore.Models;
using ScadaCore.Repositories;

namespace ScadaCore.Services;

public class UserService : IUserService
{
    public IUserRepository userRepository;

    public UserService(IUserRepository userRepository)
    {
        this.userRepository = userRepository;
    }

    public async Task<User?> GetUserAsync(string username)
    {
        var task = userRepository.GetUserAsync(username);
        task.Wait();
        return task.Result;
    }

    public async Task<User?> CreateUserAsync(User user)
    {
        var task = userRepository.CreateUserAsync(user);
        task.Wait();
        return task.Result;
    }
}