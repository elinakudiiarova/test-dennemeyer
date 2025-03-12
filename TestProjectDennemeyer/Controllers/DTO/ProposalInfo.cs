using System.ComponentModel.DataAnnotations;

namespace TestProjectDennemeyer.Controllers.DTO;

/// <summary>
/// Represents detailed information about a proposal.
/// </summary>
public class ProposalInfo
{
    /// <summary>
    /// The unique identifier of the proposal.
    /// </summary>
    /// <example>101</example>
    public int ProposalId { get; set; }

    /// <summary>
    /// The user and company that created the proposal.
    /// </summary>
    /// <remarks>
    /// If the requesting user is from the same company, user details will be included;
    /// otherwise, only company information will be provided.
    /// </remarks>
    public PartyWithUser? Creator { get; set; }

    /// <summary>
    /// An optional comment attached to the proposal.
    /// </summary>
    /// <remarks>
    /// The maximum allowed length is 250 characters.
    /// </remarks>
    /// <example>This proposal is for a trade agreement.</example>
    [MaxLength(250)]
    public string? Comment { get; set; }

    /// <summary>
    /// The date and time when the proposal was created.
    /// </summary>
    /// <example>2024-03-11T14:30:00Z</example>
    public DateTime? CreatedDate { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// The list of participants involved in the proposal.
    /// </summary>
    /// <remarks>
    /// Each participant represents a company and the user who made the decision.
    /// </remarks>
    public List<Participant> Participants { get; set; } = [];
}