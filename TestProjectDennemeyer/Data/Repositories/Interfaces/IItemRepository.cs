using TestProjectDennemeyer.Data.Entities;

namespace TestProjectDennemeyer.Data.Repositories.Interfaces;

/// <summary>
/// Retrieves item data from db.
/// </summary>
public interface IItemRepository
{
    /// <summary>
    /// Retrieves item from db
    /// </summary>
    /// <param name="itemId"></param>
    /// <returns>Item</returns>
    Task<Item?> GetItemByIdAsync(int itemId);
}