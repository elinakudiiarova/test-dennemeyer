using System.Security.Authentication;
using TestProjectDennemeyer.Data.Entities;
using TestProjectDennemeyer.Data.Repositories.Interfaces;
using TestProjectDennemeyer.Services.Interfaces;

namespace TestProjectDennemeyer.Services;

/// <summary>
/// Provides operations for managing users.
/// </summary>
public class UserService: IUserService
{
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<User> GetUserByIdAsync(int userId)
    {
        var user = await _userRepository.GetUserWithPartyAsync(userId);
        if (user is null)
        {
            throw new AuthenticationException("User does not exist.");
        }
        
        return user;
    }
}