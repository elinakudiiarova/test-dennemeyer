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
    
    /// <summary>
    /// Retrieves item from db
    /// </summary>
    /// <param name="itemId"></param>
    /// <returns>Item</returns>
    public async Task<Item?> GetItemByIdAsync(int itemId)
    {
        return await _context.Items
            .FirstOrDefaultAsync(i => i.Id == itemId);
    }
    
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
    public async Task<List<Item>> GetItemsByPartyAsync(int partyId, string? name, DateTime? fromDate, DateTime? toDate, bool? shared, string? sortBy)
    {
        var query = _context.Items
            .Include(i => i.OwnerParty) // ✅ Ensure OwnerParty is loaded
            .Include(i => i.Proposals)
            .ThenInclude(p => p.ProposalParties)
            .Where(i => i.OwnerPartyId == partyId || 
                        i.Proposals.Any(p => p.Closed == true && p.ProposalParties.All(pp => pp.Accepted == true)))
            .AsQueryable();

        if (!string.IsNullOrEmpty(name))
        {
            query = query.Where(i => i.Name.Contains(name));
        }

        if (fromDate.HasValue)
        {
            query = query.Where(i => i.CreationDate >= fromDate.Value);
        }

        if (toDate.HasValue)
        {
            query = query.Where(i => i.CreationDate <= toDate.Value);
        }

        if (shared.HasValue)
        {
            query = shared.Value
                ? query.Where(i => i.Proposals.Any(p => p.ProposalParties.Any(pp => pp.PartyId != i.OwnerPartyId)))
                : query.Where(i => i.OwnerPartyId == partyId);
        }

        query = sortBy switch
        {
            "name" => query.OrderBy(i => i.Name),
            "creationDate" => query.OrderBy(i => i.CreationDate),
            "shared" => query.OrderBy(i => i.Proposals.Any(p => p.ProposalParties.Any(pp => pp.PartyId != i.OwnerPartyId))),
            _ => query.OrderBy(i => i.Id)
        };

        return await query.ToListAsync();
    }
}