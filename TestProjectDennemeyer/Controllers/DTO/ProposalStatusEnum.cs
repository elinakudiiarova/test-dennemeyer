namespace TestProjectDennemeyer.Controllers.DTO;

/// <summary>
/// Represents the possible statuses of a proposal.
/// </summary>
public enum ProposalStatusEnum
{
    /// <summary>
    /// The proposal is awaiting a decision.
    /// </summary>
    Pending,

    /// <summary>
    /// The proposal has been approved.
    /// </summary>
    Approved,

    /// <summary>
    /// The proposal has been rejected.
    /// </summary>
    Rejected
}