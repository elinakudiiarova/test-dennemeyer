using TestProjectDennemeyer.Controllers.DTO;
using TestProjectDennemeyer.Data.Entities;
using TestProjectDennemeyer.Data.Repositories.Interfaces;
using TestProjectDennemeyer.Services.Interfaces;

namespace TestProjectDennemeyer.Services;

/// <summary>
/// Provides operations for managing proposals.
/// </summary>
public class ProposalService: IProposalService
{
    private readonly IProposalRepository _proposalRepository;

    /// <summary>
    /// Initializes a new instance of the <see cref="ProposalService"/> class.
    /// </summary>
    /// <param name="proposalRepository">The repository responsible for proposal data operations.</param>
    public ProposalService(IProposalRepository proposalRepository)
    {
        _proposalRepository = proposalRepository;
    }

    /// <summary>
    /// Retrieves all proposals associated with a specific item, filtering them based on the user's party.
    /// </summary>
    /// <param name="itemId">The ID of the item for which proposals should be retrieved.</param>
    /// <param name="userPartyId">The ID of the user's party to filter proposals.</param>
    /// <returns>A list of proposals linked to the item, or an empty list if no matching proposals exist.</returns>
    public async Task<List<Proposal>?> GetAllProposalsByItemAsync(int itemId, int userPartyId)
    {
        var proposals = await _proposalRepository.GetAllProposalsByItemAsync(itemId);
        if (proposals == null) return [];
        
        var isPartyInProposal = proposals.Any(proposal =>
            proposal.ProposalParties.Any(pp => pp.PartyId == userPartyId)
        );
        
        return !isPartyInProposal ? [] : proposals;
    }

    /// <summary>
    /// Creates a new proposal for an item.
    /// </summary>
    /// <param name="proposalRequest">The request containing proposal details.</param>
    /// <param name="user">The user who is creating the proposal.</param>
    /// <param name="item">The item associated with the proposal.</param>
    /// <returns>The created proposal entity.</returns>
    /// <exception cref="InvalidOperationException">Thrown if an active proposal already exists for the item.</exception>
    public async Task<Proposal> CreateProposalAsync(CreateProposalRequest proposalRequest, User user, Item item)
    {
        var existingProposals = await _proposalRepository.CheckForActiveProposal(item.Id);
        if (existingProposals)
        {
            throw new InvalidOperationException("A proposal already exists for this item. Counter-proposals must be submitted instead.");
        }
        
        var proposalParties = HandleAndValidateProposalParty(proposalRequest, item);
        
        var proposal = new Proposal
        {
            ItemId = proposalRequest.ItemId,
            Comment = proposalRequest.Comment,
            ProposalParties = proposalParties,
            CreatedDate = DateTime.UtcNow,
            CreatorId = user.Id
        };
        
        var createdProposal = await _proposalRepository.CreateProposalAsync(proposal);
        
        return createdProposal;
    }

    /// <summary>
    /// Creates a counter-proposal in response to an existing proposal.
    /// </summary>
    /// <param name="proposalRequest">The counter-proposal request.</param>
    /// <param name="user">The user submitting the counter-proposal.</param>
    /// <param name="item">The item associated with the proposal.</param>
    /// <param name="initialProposalId">The ID of the initial proposal being countered.</param>
    /// <returns>The newly created counter-proposal.</returns>
    /// <exception cref="InvalidOperationException">Thrown if validation checks fail.</exception>
    public async Task<Proposal?> CreateCounterProposalAsync(CreateCounterProposalRequest proposalRequest, User user, Item item,
        int initialProposalId)
    {
        var existingProposals = await _proposalRepository.CheckForActiveProposal(item.Id);
        if (!existingProposals)
        {
            throw new InvalidOperationException("A proposal does not exist for this item. New proposal must be submitted instead.");
        }

        var initialProposal = await _proposalRepository.GetProposalByIdAsync(initialProposalId);
        ValidateInitialProposal(initialProposal!, item.Id, user.PartyId);
        CompareCompanies(initialProposal!, proposalRequest);
        
        var proposalParties = HandleAndValidateProposalParty(proposalRequest, item);
        var counterProposal = new Proposal
        {
            ItemId = proposalRequest.ItemId,
            Comment = proposalRequest.Comment,
            ProposalParties = proposalParties,
            CreatedDate = DateTime.UtcNow,
            CreatorId = user.Id
        };

        var createdCounterProposal = await _proposalRepository.ReplaceProposalAsync(counterProposal, initialProposal!);

        return createdCounterProposal;
    }

    /// <summary>
    /// Finalizes the decision on an existing proposal by either accepting or rejecting it.
    /// </summary>
    /// <param name="request">The proposal decision request.</param>
    /// <param name="user">The user making the decision.</param>
    /// <param name="item">The item associated with the proposal.</param>
    /// <param name="initialProposalId">The ID of the proposal being decided on.</param>
    /// <returns>The updated proposal after the decision has been applied.</returns>
    public async Task<Proposal?> FinilizeProposalDecisionAsync(ProposalDecisionRequest request, User user, Item item,
        int initialProposalId)
    {
        var existingProposals = await _proposalRepository.CheckForActiveProposal(item.Id);
        if (!existingProposals)
        {
            throw new InvalidOperationException("A proposal does not exist for this item. New proposal must be submitted instead.");
        }
        
        var initialProposal = await _proposalRepository.GetProposalByIdAsync(initialProposalId);
        ValidateInitialProposal(initialProposal!, item.Id, user.PartyId);
        
        if (request.Decision)
        {
            initialProposal!.Closed = true;
            await _proposalRepository.UpdateDecisionAndCheckFinalizationAsync(initialProposal, user.PartyId, user.Id, item);
            return initialProposal;
        }
        
        await _proposalRepository.UpdateProposalDecisionAsync(initialProposal!, user.PartyId, user.Id, false);
        CompareCompanies(initialProposal!, request);
        var proposalParties = HandleAndValidateProposalParty(request, item);
        var counterProposal = new Proposal
        {
            ItemId = request.ItemId,
            Comment = request.Comment,
            ProposalParties = proposalParties,
            CreatedDate = DateTime.UtcNow,
            CreatorId = user.Id
        };

        var createdCounterProposal = await _proposalRepository.ReplaceProposalAsync(counterProposal, initialProposal!);

        return createdCounterProposal;
    }
    
    private decimal CountProposalAmount(decimal? percentage, decimal totalAmount)
    {
        if (!percentage.HasValue || percentage.Value <= 0 || percentage.Value > 100)
        {
            throw new ArgumentException("Percentage must be between 0 and 100.");
        }

        return totalAmount * (percentage.Value / 100);
    }

    private List<ProposalParty> HandleAndValidateProposalParty(CreateProposalRequest proposalRequest, Item item)
    {
        var proposalParties = proposalRequest.PartyShare.Select(p => new ProposalParty
        {
            PartyId = p.PartyId,
            Amount = p.Amount ?? CountProposalAmount(p.Percentage, item.Value),
            Percentage = p.Percentage
        }).ToList();
        
        var totalAssignedAmount = proposalParties.Sum(p => p.Amount);
        if (totalAssignedAmount > item.Value)
        {
            throw new ArgumentException("The total assigned amount cannot exceed the item's value.");
        }

        return proposalParties;
    }

    private void CompareCompanies(Proposal initialProposal, CreateProposalRequest proposalRequest)
    {
        var initialProposalParties = initialProposal.ProposalParties.Select(pp => pp.PartyId).ToHashSet();
        var counterProposalParties = proposalRequest.PartyShare.Select(p => p.PartyId).ToHashSet();
        
        if (!initialProposalParties.SetEquals(counterProposalParties))
        {
            throw new InvalidOperationException("Counter-proposal must include the same companies as the initial proposal.");
        }
    }

    private void ValidateInitialProposal(Proposal initialProposal, int itemId, int userPartyId)
    {
        if (initialProposal == null || initialProposal.Closed)
        {
            throw new InvalidOperationException("The referenced initial proposal does not exist or is closed.");
        }
                
        if (initialProposal.ItemId != itemId)
        {
            throw new InvalidOperationException("The referenced proposal does not belong to the specified item.");
        }

        if (initialProposal.Creator != null && initialProposal.Creator.PartyId == userPartyId)
        {
            throw new InvalidOperationException("You cannot make decisions on your own proposal.");
        }
    }
}