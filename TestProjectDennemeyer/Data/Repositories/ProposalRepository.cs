using Microsoft.EntityFrameworkCore;
using TestProjectDennemeyer.Data.Entities;
using TestProjectDennemeyer.Data.Repositories.Interfaces;

namespace TestProjectDennemeyer.Data.Repositories;

/// <summary>
/// This repository retrieves proposal data from db.
/// </summary>
public class ProposalRepository: IProposalRepository
{
    private readonly TestDennemeyerDbContext _context;

    public ProposalRepository(TestDennemeyerDbContext context)
    {
        _context = context;
    }
    
    /// <summary>
    /// Retrieves proposals with parties' decisions from db
    /// </summary>
    /// <param name="itemId"></param>
    /// <returns></returns>
    public async Task<List<Proposal>?> GetAllProposalsByItemAsync(int itemId)
    {
        return await _context.Proposals
            .Where(p => p.ItemId == itemId)
            .Include(p => p.Creator)
            .ThenInclude(u => u.Party)
            .Include(p => p.ProposalParties)
            .ThenInclude(pp => pp.DecisionUser)
            .Include(p => p.ProposalParties)
            .ThenInclude(pp => pp.Party)
            .OrderBy(p => p.ProposalParties.Min(pp => pp.Party.CreationDate))    
            .ToListAsync();
    }
}