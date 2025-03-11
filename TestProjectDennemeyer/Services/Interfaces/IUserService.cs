using TestProjectDennemeyer.Data.Entities;

namespace TestProjectDennemeyer.Services.Interfaces;

/// <summary>
/// Provides operations for managing users.
/// </summary>
public interface IUserService
{
    /// <summary>
    /// Retrieves user by id.
    /// </summary>
    /// <param name="userId">ID of user.</param>
    /// <returns>Retries user including party.</returns>
    Task<User> GetUserByIdAsync(int userId);
}