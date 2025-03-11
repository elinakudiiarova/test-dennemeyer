using TestProjectDennemeyer.Data.Entities;

namespace TestProjectDennemeyer.Data.Repositories.Interfaces;

/// <summary>
///  Retrieves user data from db.
/// </summary>
public interface IUserRepository
{
    /// <summary>
    /// Retrieves user with a party from db
    /// </summary>
    /// <param name="userId"></param>
    /// <returns>User</returns>
    Task<User?> GetUserWithPartyAsync(int userId);
}