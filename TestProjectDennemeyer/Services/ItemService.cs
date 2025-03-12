using TestProjectDennemeyer.Data.Entities;
using TestProjectDennemeyer.Data.Repositories.Interfaces;
using TestProjectDennemeyer.Services.Interfaces;

namespace TestProjectDennemeyer.Services;

/// <summary>
/// Service class responsible for managing item-related operations.
/// </summary>
public class ItemService: IItemService
{
    private readonly IItemRepository _itemRepository;

    /// <summary>
    /// Initializes a new instance of the <see cref="ItemService"/> class.
    /// </summary>
    /// <param name="itemRepository">The repository for item data operations.</param>
    /// <param name="userService">The service for user operations.</param>
    public ItemService(IItemRepository itemRepository)
    {
        _itemRepository = itemRepository;
    }
    
    /// <summary>
    /// Retrieves an item by its ID.
    /// </summary>
    /// <param name="itemId">The ID of the item to retrieve.</param>
    /// <returns>
    /// The <see cref="Item"/> entity if found; otherwise, <c>null</c>.
    /// </returns>
    public async Task<Item?> GetItemByIdAsync(int itemId)
    {
        var item = await _itemRepository.GetItemByIdAsync(itemId);
        return item; 
    }

    /// <summary>
    /// Retrieves a list of items associated with the specified user.
    /// </summary>
    /// <param name="partyId">The ID of the party which items should be retrieved.</param>
    /// <param name="name">Filter to search for items by name.</param>
    /// <param name="fromDate">Filter to retrieve items created on or after this date.</param>
    /// <param name="toDate">Filter to retrieve items created on or before this date.</param>
    /// <param name="shared">Filter to specify whether to retrieve only shared or owned items.</param>
    /// <param name="sortBy">Sorting criteria (e.g., "name", "creationDate", or "shared").</param>
    /// <returns>The list items.</returns>
    public async Task<List<Item>?> GetItemsForUserAsync(int partyId, string? name, DateTime? fromDate, DateTime? toDate, bool? shared, string? sortBy)
    {
        var items = await _itemRepository.GetItemsByPartyAsync(partyId, name, fromDate, toDate, shared, sortBy);
        return items;
    }
}