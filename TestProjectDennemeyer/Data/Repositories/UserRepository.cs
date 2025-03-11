using Microsoft.EntityFrameworkCore;
using TestProjectDennemeyer.Data.Entities;
using TestProjectDennemeyer.Data.Repositories.Interfaces;

namespace TestProjectDennemeyer.Data.Repositories;

/// <summary>
/// This repository retrieves user data from db.
/// </summary>
public class UserRepository: IUserRepository
{
    private readonly TestDennemeyerDbContext _context;

    public UserRepository(TestDennemeyerDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Retrieves user with a party from db
    /// </summary>
    /// <param name="userId"></param>
    /// <returns>User</returns>
    public async Task<User?> GetUserWithPartyAsync(int userId)
    {
        return await _context.Users
            .Include(u => u.Party)
            .FirstOrDefaultAsync(u => u.Id == userId);
    }
}