using System.ComponentModel.DataAnnotations;
using TestProjectDennemeyer.Controllers.Validators;

namespace TestProjectDennemeyer.Controllers.DTO;

/// <summary>
/// Represents a request to create a counter-proposal.
/// </summary>
public class CreateCounterProposalRequest: CreateProposalRequest
{
    /// <summary>
    /// A comment for the counter-proposal.
    /// </summary>
    /// <remarks>
    /// The maximum length is 250 characters. Is required.
    /// </remarks>
    /// <example>This is a sample proposal comment.</example>\
    [Required(ErrorMessage = "Comment is required.")]
    [StringLength(250, ErrorMessage = "Comment must be at most 250 characters.")]
    public override string Comment { get; set; }
    
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
    public override List<PartyShare> PartyShare { get; set; } = [];
}