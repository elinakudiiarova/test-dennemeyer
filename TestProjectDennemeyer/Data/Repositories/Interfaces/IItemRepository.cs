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

    /// <summary>
    /// Retrieves items connected to user party from db
    /// </summary>
    /// <param name="partyId">ID of the party.</param>
    /// <param name="name">Filter by name</param>
    /// <param name="fromDate">Filter from date</param>
    /// <param name="toDate">Filter to date</param>
    /// <param name="shared">Filter by shared</param>
    /// <param name="sortBy">Name of field you want to sort by</param>
    /// <returns>List of items</returns>
    Task<List<Item>> GetItemsByPartyAsync(int partyId, string? name, DateTime? fromDate, DateTime? toDate,
        bool? shared, string? sortBy);
}