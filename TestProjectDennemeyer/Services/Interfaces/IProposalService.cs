using TestProjectDennemeyer.Controllers.DTO;
using TestProjectDennemeyer.Data.Entities;

namespace TestProjectDennemeyer.Services.Interfaces;

/// <summary>
/// Provides operations for managing proposals.
/// </summary>
public interface IProposalService
{
    /// <summary>
    /// Retrieves all proposals by item.
    /// </summary>
    /// <param name="itemId">ID of the item</param>
    /// <param name="userPartyId">ID of current user's party.</param>
    /// <returns>A list of proposals connected to the item.</returns>
    Task<List<Proposal>?> GetAllProposalsByItemAsync(int itemId, int userPartyId);

    /// <summary>
    /// Create a new proposal.
    /// </summary>
    /// <param name="proposalRequest">New proposal request.</param>
    /// <param name="user">Creator of proposal.</param>
    /// <param name="item">Shared item.</param>
    /// <returns>Created proposal.</returns>
    Task<Proposal> CreateProposalAsync(CreateProposalRequest proposalRequest, User user, Item item);
    
    /// <summary>
    /// Create a new counter-proposal.
    /// </summary>
    /// <param name="proposalRequest">New proposal request.</param>
    /// <param name="user">Creator of proposal.</param>
    /// <param name="item">Shared item.</param>
    /// <param name="initialProposalId">The ID of the initial proposal.</param>
    /// <returns>Created proposal.</returns>
    Task<Proposal> CreateCounterProposalAsync(CreateCounterProposalRequest proposalRequest, User user, Item item, int initialProposalId);
}