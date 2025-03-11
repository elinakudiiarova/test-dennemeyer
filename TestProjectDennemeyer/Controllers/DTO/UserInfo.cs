using System.ComponentModel.DataAnnotations;

namespace TestProjectDennemeyer.Controllers.DTO;

/// <summary>
/// This DTO represents information about the user
/// </summary>
public class UserInfo
{
    /// <summary>
    /// User id.
    /// </summary>
    public int? UserId { get; set; }
    /// <summary>
    /// User first name
    /// </summary>
    [MaxLength(80)]
    public string? Name { get; set; }
    /// <summary>
    /// User second name
    /// </summary>
    [MaxLength(100)]
    public string? Surname { get; set; }
}