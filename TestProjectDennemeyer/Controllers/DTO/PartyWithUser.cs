namespace TestProjectDennemeyer.Controllers.DTO;

/// <summary>
/// This DTO represents information about the company, and it's connected to the action user
/// </summary>
public class PartyWithUser
{
    /// <summary>
    /// Party information
    /// </summary>
    public PartyInfo Party { get; set; }
    /// <summary>
    /// User that represents action done by the party
    /// </summary>
    public UserInfo? User { get; set; }
}