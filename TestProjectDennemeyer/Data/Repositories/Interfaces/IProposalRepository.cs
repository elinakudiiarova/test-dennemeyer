using TestProjectDennemeyer.Data.Entities;

namespace TestProjectDennemeyer.Data.Repositories.Interfaces;

/// <summary>
/// Retrieves proposal data from db.
/// </summary>
public interface IProposalRepository
{
    /// <summary>
    /// Retrieves proposals with parties decision from db.
    /// </summary>
    /// <param name="itemId">Item id.</param>
    /// <returns>List of proposals</returns>
    Task<List<Proposal>?> GetAllProposalsByItemAsync(int itemId);

    /// <summary>
    /// Creates proposal.
    /// </summary>
    /// <param name="proposal">Proposal entity that will be saved.</param>
    /// <returns>Created proposal</returns>
    Task<Proposal> CreateProposalAsync(Proposal proposal);
    
    /// <summary>
    /// Gives information on whether an item has active proposals.
    /// </summary>
    /// <param name="itemId">Item id.</param>
    /// <returns>Boolean value for item having active proposal</returns>
    Task<bool> CheckForActiveProposal(int itemId);

    /// <summary>
    /// Retrieves proposal by ID.
    /// </summary>
    /// <param name="id">ID which will be used to find proposal.</param>
    /// <returns>Proposal</returns>
    Task<Proposal?> GetProposalByIdAsync(int id);

    /// <summary>
    /// Adds a new proposal to the database and changes previous one to closed.
    /// </summary>
    /// <param name="newProposal">The proposal entity to be added.</param>
    /// <param name="initialProposal">The initial proposal that will be marked as closed.</param>
    /// <returns>
    /// The created proposal entity with its generated ID and related data.
    /// </returns>
    Task<Proposal?> ReplaceProposalAsync(Proposal newProposal, Proposal initialProposal);
}