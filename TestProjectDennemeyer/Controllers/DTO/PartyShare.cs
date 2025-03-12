using System.ComponentModel.DataAnnotations;

namespace TestProjectDennemeyer.Controllers.DTO;

/// <summary>
/// Represents a party's share in a proposal. 
/// Either <see cref="Amount"/> or <see cref="Percentage"/> must be provided, but not both.
/// </summary>
public class PartyShare
{
    /// <summary>
    /// The unique identifier of the party.
    /// </summary>
    /// <remarks>
    /// This value must be greater than 0.
    /// </remarks>
    /// <example>1</example>
    [Required(ErrorMessage = "PartyId is required.")]
    [Range(1, int.MaxValue, ErrorMessage = "PartyId must be greater than 0.")]
    public int PartyId { get; set; }

    /// <summary>
    /// The monetary amount allocated to the party.
    /// </summary>
    /// <remarks>
    /// This value must be greater than 0. If this field is provided, <see cref="Percentage"/> must be omitted.
    /// </remarks>
    /// <example>1500.75</example>
    [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be greater than 0.")]
    public decimal? Amount { get; set; }

    /// <summary>
    /// The percentage of the total proposal allocated to this party.
    /// </summary>
    /// <remarks>
    /// This value must be between 0 and 100. If this field is provided, <see cref="Amount"/> must be omitted.
    /// </remarks>
    /// <example>50.0</example>
    [Range(0.01, 100, ErrorMessage = "Percentage must be between 0 and 100.")]
    public decimal? Percentage { get; set; }
}