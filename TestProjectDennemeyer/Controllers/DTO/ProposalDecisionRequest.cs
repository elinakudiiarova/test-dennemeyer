using System.ComponentModel.DataAnnotations;
using TestProjectDennemeyer.Controllers.Validators;

namespace TestProjectDennemeyer.Controllers.DTO;

/// <summary>
/// Represents a request to make a decision on a proposal.
/// </summary>
[ValidateProposalDecision] 
public class ProposalDecisionRequest : CreateProposalRequest
{
    /// <summary>
    /// Indicates whether the proposal is approved (true) or rejected (false).
    /// </summary>
    /// <example>true</example>
    [Required(ErrorMessage = "Decision is required.")]
    public required bool Decision { get; set; }

    /// <summary>
    /// An optional comment for the proposal.
    /// Required when rejecting a proposal.
    /// </summary>
    /// <remarks>
    /// The maximum length is 250 characters.
    /// </remarks>
    /// <example>This is a sample proposal comment. Required when you reject proposal.</example>
    [StringLength(250, ErrorMessage = "Comment must be at most 250 characters.")]
    public override string? Comment { get; set; }

    /// <summary>
    /// List of parties involved in the counter-proposal.
    /// Required when rejecting a proposal.
    /// </summary>
    [RequiredIfRejected]
    public override List<PartyShare>? PartyShare { get; set; }
}