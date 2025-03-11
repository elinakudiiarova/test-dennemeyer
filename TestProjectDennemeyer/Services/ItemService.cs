using TestProjectDennemeyer.Data.Entities;
using TestProjectDennemeyer.Data.Repositories.Interfaces;
using TestProjectDennemeyer.Services.Interfaces;

namespace TestProjectDennemeyer.Services;

public class ItemService: IItemService
{
    private readonly IItemRepository _itemRepository;

    public ItemService(IItemRepository itemRepository)
    {
        _itemRepository = itemRepository;
    }
    public async Task<Item?> GetItemByIdAsync(int itemId)
    {
        var item = await _itemRepository.GetItemByIdAsync(itemId);
        return item; 
    }
}