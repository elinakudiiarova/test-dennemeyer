using TestProjectDennemeyer.Data.Entities;

namespace TestProjectDennemeyer.Data.Repositories.Interfaces;

/// <summary>
/// Retrieves proposal data from db.
/// </summary>
public interface IProposalRepository
{
    /// <summary>
    /// Retrieves proposals with parties decision from db
    /// </summary>
    /// <param name="itemId"></param>
    /// <returns>List of proposals</returns>
    Task<List<Proposal>?> GetAllProposalsByItemAsync(int itemId);
}