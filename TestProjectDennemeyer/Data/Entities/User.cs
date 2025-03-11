using System.ComponentModel.DataAnnotations;

namespace TestProjectDennemeyer.Data.Entities;

public class User
{
    public int Id { get; set; }
    [MaxLength(80)]
    public string Name { get; set; } = String.Empty;
    [MaxLength(100)]
    public string Surname { get; set; } = String.Empty;
    public Party Party { get; set; } = null!;
    public int PartyId { get; set; }
    public ICollection<ProposalParty>? ProposalParties { get; set; }
}