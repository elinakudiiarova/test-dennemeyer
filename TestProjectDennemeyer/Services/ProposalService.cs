using System.Security.Authentication;
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

    public ProposalService(IProposalRepository proposalRepository)
    {
        _proposalRepository = proposalRepository;
    }

    public async Task<List<Proposal>?> GetAllProposalsByItemAsync(int itemId, int userPartyId)
    {
        var proposals = await _proposalRepository.GetAllProposalsByItemAsync(itemId);
        if (proposals == null) return [];
        
        var isPartyInProposal = proposals.Any(proposal =>
            proposal.ProposalParties.Any(pp => pp.PartyId == userPartyId)
        );
        
        return !isPartyInProposal ? [] : proposals;
    }

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

    public async Task<Proposal> CreateCounterProposalAsync(CreateCounterProposalRequest proposalRequest, User user, Item item,
        int initialProposalId)
    {
        var existingProposals = await _proposalRepository.CheckForActiveProposal(item.Id);
        if (!existingProposals)
        {
            throw new InvalidOperationException("A proposal does not exist for this item. New proposal must be submitted instead.");
        }
        
        var initialProposal = await _proposalRepository.GetProposalByIdAsync(initialProposalId);
        if (initialProposal == null || initialProposal.Closed)
        {
            throw new InvalidOperationException("The referenced initial proposal does not exist or is closed.");
        }
                
        if (initialProposal.ItemId != item.Id)
        {
            throw new InvalidOperationException("The referenced proposal does not belong to the specified item.");
        }

        if (initialProposal.Creator != null && initialProposal.Creator.PartyId == user.PartyId)
        {
            throw new InvalidOperationException("You cannot create a counter-proposal to your own proposal.");
        }
        
        CompareCompanies(initialProposal, proposalRequest);
        
        var proposalParties = HandleAndValidateProposalParty(proposalRequest, item);
        var counterProposal = new Proposal
        {
            ItemId = proposalRequest.ItemId,
            Comment = proposalRequest.Comment,
            ProposalParties = proposalParties,
            CreatedDate = DateTime.UtcNow,
            CreatorId = user.Id
        };

        var createdCounterProposal = await _proposalRepository.ReplaceProposalAsync(counterProposal, initialProposal);

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
}