using System.ComponentModel.DataAnnotations;

namespace TestProjectDennemeyer.Controllers.DTO;

/// <summary>
/// Represents information about a company or party in the system.
/// </summary>
public class PartyInfo
{
    /// <summary>
    /// ID of the party.
    /// </summary>
    /// <example>1</example>
    public int PartyId { get; set; }

    /// <summary>
    /// The name of the company.
    /// </summary>
    /// <remarks>
    /// The maximum length allowed is 150 characters.
    /// </remarks>
    /// <example>Alpha Corporation</example>
    [MaxLength(150)]
    public string PartyName { get; set; } = string.Empty;
}