using System.ComponentModel.DataAnnotations;
using TestProjectDennemeyer.Controllers.Validators;

namespace TestProjectDennemeyer.Controllers.DTO;

/// <summary>
/// Represents a request to create a proposal.
/// </summary>
public class CreateProposalRequest
{
    /// <summary>
    /// The ID of the item associated with the proposal.
    /// </summary>
    /// <remarks>
    /// This value must be greater than 0.
    /// </remarks>
    /// <example>5</example>
    [Required(ErrorMessage = "ItemId is required.")]
    [Range(1, int.MaxValue, ErrorMessage = "ItemId must be greater than 0.")]
    public int ItemId { get; set; }

    /// <summary>
    /// An optional comment for the proposal.
    /// </summary>
    /// <remarks>
    /// The maximum length is 250 characters.
    /// </remarks>
    /// <example>This is a sample proposal comment.</example>
    [StringLength(250, ErrorMessage = "Comment must be at most 250 characters.")]
    public virtual string? Comment { get; set; }
    
    /// <summary>
    /// The list of parties participating in the proposal, along with their share details.
    /// </summary>
    /// <remarks>
    /// This list must contain at least one entry.
    /// </remarks>
    /// <example>
    /// [
    ///   {
    ///     "PartyId": 1,
    ///     "Percentage": 50.0,
    ///     "Amount": 1000.00
    ///   },
    ///   {
    ///     "PartyId": 2,
    ///     "Percentage": 50.0,
    ///     "Amount": 1000.00
    ///   }
    /// ]
    /// </example>
    [Required(ErrorMessage = "Parties with share are required.")]
    [OnlyOneShareRequired]
    public virtual List<PartyShare> PartyShare { get; set; } = [];
}