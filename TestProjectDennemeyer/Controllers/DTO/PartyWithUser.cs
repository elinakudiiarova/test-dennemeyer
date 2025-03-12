namespace TestProjectDennemeyer.Controllers.DTO;

/// <summary>
/// Represents information about a party and the user associated with its actions.
/// </summary>
public class PartyWithUser
{
    /// <summary>
    /// Information about the party.
    /// </summary>
    public required PartyInfo Party { get; set; }

    /// <summary>
    /// The user representing the party's action in a proposal.
    /// </summary>
    public UserInfo? User { get; set; }
}