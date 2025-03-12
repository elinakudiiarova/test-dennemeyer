namespace TestProjectDennemeyer.Controllers.DTO;

/// <summary>
/// Represents the response for a proposal history request.
/// </summary>
public class ProposalHistoryResponse
{
    /// <summary>
    /// The item associated with the proposals.
    /// </summary>
    public required ItemInfo Item { get; set; }

    /// <summary>
    /// The list of proposals related to the item.
    /// </summary>
    /// <remarks>
    /// This list may be empty if no proposals exist for the item.
    /// </remarks>
    public List<ProposalInfo>? Proposals { get; set; }
}