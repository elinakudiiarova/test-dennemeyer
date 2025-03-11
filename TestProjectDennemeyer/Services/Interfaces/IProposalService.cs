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
}