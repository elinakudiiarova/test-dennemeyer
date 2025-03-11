using System.ComponentModel.DataAnnotations;

namespace TestProjectDennemeyer.Controllers.DTO;

/// <summary>
/// This DTO represents information about the company
/// </summary>
public class PartyInfo
{
    /// <summary>
    /// Party id
    /// </summary>
    public int PartyId { get; set; }
    /// <summary>
    /// Company name
    /// </summary>
    [MaxLength(150)]
    public string PartyName { get; set; } = String.Empty;
}