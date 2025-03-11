using TestProjectDennemeyer.Data.Entities;

namespace TestProjectDennemeyer.Services.Interfaces;

/// <summary>
/// Provides operations for managing items.
/// </summary>
public interface IItemService
{
    /// <summary>
    /// Retrieves item by id.
    /// </summary>
    /// <param name="itemId">ID of the item.</param>
    /// <returns>An item.</returns>
    Task<Item?> GetItemByIdAsync(int itemId);
}