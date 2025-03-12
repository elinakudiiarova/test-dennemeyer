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

    /// <summary>
    /// Initializes a new instance of the <see cref="UserService"/> class.
    /// </summary>
    /// <param name="userRepository">The repository responsible for user data operations.</param>
    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    /// <summary>
    /// Retrieves a user by their ID, including their associated party details.
    /// </summary>
    /// <param name="userId">The ID of the user to retrieve.</param>
    /// <returns>The user entity if found.</returns>
    /// <exception cref="AuthenticationException">Thrown if the user does not exist.</exception>
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