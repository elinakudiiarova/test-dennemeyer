using System.ComponentModel.DataAnnotations;

namespace TestProjectDennemeyer.Controllers.DTO;

/// <summary>
/// Represents information about a user.
/// </summary>
public class UserInfo
{
    /// <summary>
    /// The unique identifier of the user.
    /// </summary>
    /// <remarks>
    /// This value may be <c>null</c> if user identification is not required.
    /// </remarks>
    /// <example>123</example>
    public int? UserId { get; set; }

    /// <summary>
    /// The first name of the user.
    /// </summary>
    /// <remarks>
    /// The maximum allowed length is 80 characters.
    /// </remarks>
    /// <example>John</example>
    [MaxLength(80)]
    public string? Name { get; set; }

    /// <summary>
    /// The last name (surname) of the user.
    /// </summary>
    /// <remarks>
    /// The maximum allowed length is 100 characters.
    /// </remarks>
    /// <example>Doe</example>
    [MaxLength(100)]
    public string? Surname { get; set; }
}