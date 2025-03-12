using System.Text.Json.Serialization;

namespace TestProjectDennemeyer.Controllers.DTO;

/// <summary>
/// Represents a participant in a proposal.
/// </summary>
public class Participant
{
    /// <summary>
    /// The decision status of the participant regarding the proposal.
    /// </summary>
    /// <remarks>
    /// Can be <c>Approved</c>, <c>Rejected</c>, or <c>Pending</c>.
    /// </remarks>
    /// <example>Approved</example>
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public ProposalStatusEnum? DecisionStatus { get; set; }

    /// <summary>
    /// The amount proposed by the participant.
    /// </summary>
    /// <remarks>
    /// This value must be a positive decimal number.
    /// </remarks>
    /// <example>1500.75</example>
    public decimal PurposedAmount { get; set; }

    /// <summary>
    /// The percentage of the total proposal allocated to this participant.
    /// </summary>
    /// <remarks>
    /// This value, if provided, must be between 0 and 100.
    /// </remarks>
    /// <example>50.0</example>
    public decimal? PurposedPercentage { get; set; }

    /// <summary>
    /// Information about the party and the user representing this participant.
    /// </summary>
    public required PartyWithUser Party { get; set; }
}