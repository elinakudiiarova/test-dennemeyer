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

    /// <summary>
    /// Initializes a new instance of the ProposalRepository class.
    /// </summary>
    /// <param name="context">The database context used for accessing proposal data.</param>
    public ProposalRepository(TestDennemeyerDbContext context)
    {
        _context = context;
    }
    
    /// <summary>
    /// Retrieves all proposals associated with a specific item, including party decisions.
    /// </summary>
    /// <param name="itemId">The ID of the item for which proposals should be retrieved.</param>
    /// <returns>
    /// A list of proposals objects associated with the specified item.
    /// Returns an empty list if no proposals exist.
    /// </returns>
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

    /// <summary>
    /// Adds a new proposal to the database.
    /// </summary>
    /// <param name="proposal">The proposal entity to be added.</param>
    /// <returns>
    /// The created proposal entity with its generated ID and related data.
    /// </returns>
    public async Task<Proposal?> CreateProposalAsync(Proposal proposal)
    {
        _context.Proposals.Add(proposal);
        await _context.SaveChangesAsync();
        
        return await GetProposalByIdAsync(proposal.Id);
    }
    
    /// <summary>
    /// Checks if an active proposal exists for a given item.
    /// </summary>
    /// <param name="itemId">The ID of the item to check.</param>
    /// <returns>
    /// <c>true</c> if at least one proposal exists for the specified item; otherwise, <c>false</c>.
    /// </returns>
    public async Task<bool> CheckForActiveProposal(int itemId)
    {
        return await _context.Proposals.AnyAsync(p => p.ItemId == itemId && p.Closed != true);
    }

    /// <summary>
    /// Retrieves proposal by ID.
    /// </summary>
    /// <param name="id">ID which will be used to find proposal.</param>
    /// <returns>Proposal</returns>
    public async Task<Proposal?> GetProposalByIdAsync(int id)
    {
        return await _context.Proposals
            .Include(p => p.Creator)
            .ThenInclude(u => u.Party)
            .Include(p => p.ProposalParties)
            .ThenInclude(pp => pp.DecisionUser)
            .Include(p => p.ProposalParties)
            .ThenInclude(pp => pp.Party)
            .FirstOrDefaultAsync(p => p.Id == id);
    }
    
    /// <summary>
    /// Adds a new proposal to the database and changes previous one to closed.
    /// </summary>
    /// <param name="newProposal">The proposal entity to be added.</param>
    /// <param name="initialProposal">The initial proposal that will be marked as closed.</param>
    /// <returns>
    /// The created proposal entity with its generated ID and related data.
    /// </returns>
    public async Task<Proposal?> ReplaceProposalAsync(Proposal newProposal, Proposal initialProposal)
    {
        await using var transaction = await _context.Database.BeginTransactionAsync();

        try
        {
            if (initialProposal == null)
            {
                throw new InvalidOperationException("The initial proposal does not exist.");
            }
            initialProposal.Closed = true;
            await _context.SaveChangesAsync();

            _context.Proposals.Add(newProposal);
            await _context.SaveChangesAsync();
            await transaction.CommitAsync(); 

            return await GetProposalByIdAsync(newProposal.Id); 
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    /// <summary>
    /// Updates a proposal decision and checks if all parties have accepted the proposal.
    /// If all parties accept, the proposal is finalized, and the item is marked as shared.
    /// </summary>
    /// <param name="proposal">The proposal entity being updated.</param>
    /// <param name="partyId">The ID of the party making the decision.</param>
    /// <param name="decisionUserId">The ID of the user making the decision.</param>
    /// <param name="item">The item associated with the proposal.</param>
    public async Task UpdateDecisionAndCheckFinalizationAsync(Proposal proposal, int partyId, int decisionUserId, Item item)
    {
        var partyProposal = proposal.ProposalParties.FirstOrDefault(pp => pp.PartyId == partyId);
        if (partyProposal == null)
        {
            throw new InvalidOperationException("Your company is not part of this proposal.");
        }

        partyProposal.Accepted = true;
        partyProposal.DecisionUserId = decisionUserId;

        bool allAccepted = proposal.ProposalParties.All(pp => pp.Accepted == true);

        if (allAccepted)
        {
            proposal.Closed = true;
            item.Shared = true;
        }

        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Updates a proposal party decision by setting acceptance or rejection status.
    /// </summary>
    /// <param name="initialProposal">The proposal being updated.</param>
    /// <param name="partyId">The ID of the party making the decision.</param>
    /// <param name="decisionUserId">The ID of the user making the decision.</param>
    /// <param name="decision">The decision value (true for approval, false for rejection).</param>
    public async Task UpdateProposalDecisionAsync(Proposal initialProposal, int partyId, int decisionUserId, bool decision)
    {
        var partyProposal = initialProposal.ProposalParties.FirstOrDefault(pp => pp.PartyId == partyId);
        if (partyProposal == null)
        {
            throw new InvalidOperationException("Your company is not part of this proposal.");
        }

        partyProposal.Accepted = decision;
        partyProposal.DecisionUserId = decisionUserId;

        await _context.SaveChangesAsync();
    }
}