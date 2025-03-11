using System.Security.Authentication;
using TestProjectDennemeyer.Data.Entities;
using TestProjectDennemeyer.Data.Repositories.Interfaces;
using TestProjectDennemeyer.Services.Interfaces;

namespace TestProjectDennemeyer.Services;

/// <summary>
/// Provides operations for managing proposals.
/// </summary>
public class ProposalService : IProposalService
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
}