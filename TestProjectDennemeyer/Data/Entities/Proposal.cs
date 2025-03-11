
using System.ComponentModel.DataAnnotations;

namespace TestProjectDennemeyer.Data.Entities;

public class Proposal
{
    public int Id { get; set; }
    public DateTime CreatedDate { get; set; }
    [MaxLength(250)]
    public string? Comment { get; set; }
    public int CreatorId { get; set; }
    public User Creator { get; set; } = null!;
    public int ItemId { get; set; }
    public Item Item { get; set; } = null!;
    public bool? Closed { get; set; } 
    public ICollection<ProposalParty> ProposalParties { get; set; } = new List<ProposalParty>();
}