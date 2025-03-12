using TestProjectDennemeyer.Controllers.DTO;
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

    /// <summary>
    /// Retrieves a list of items associated with the specified user.
    /// </summary>
    /// <param name="partyId">The ID of the party which items should be retrieved.</param>
    /// <param name="name">Filter to search for items by name.</param>
    /// <param name="fromDate">Filter to retrieve items created on or after this date.</param>
    /// <param name="toDate">Filter to retrieve items created on or before this date.</param>
    /// <param name="shared">Filter to specify whether to retrieve only shared or owned items.</param>
    /// <param name="sortBy">Sorting criteria (e.g., "name", "creationDate", or "shared").</param>
    /// <returns>A response object containing the list of items.</returns>
    Task<List<Item>?> GetItemsForUserAsync(int partyId, string? name, DateTime? fromDate, DateTime? toDate, bool? shared, string? sortBy);
}