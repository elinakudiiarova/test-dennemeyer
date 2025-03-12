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
}