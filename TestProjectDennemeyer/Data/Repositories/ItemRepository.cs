using Microsoft.EntityFrameworkCore;
using TestProjectDennemeyer.Data.Entities;
using TestProjectDennemeyer.Data.Repositories.Interfaces;

namespace TestProjectDennemeyer.Data.Repositories;

public class ItemRepository: IItemRepository
{
    private readonly TestDennemeyerDbContext _context;

    public ItemRepository(TestDennemeyerDbContext context)
    {
        _context = context;
    }

    public async Task<Item?> GetItemByIdAsync(int itemId)
    {
        return await _context.Items
            .FirstOrDefaultAsync(i => i.Id == itemId);
    }
}