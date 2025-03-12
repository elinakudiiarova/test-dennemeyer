
using System.ComponentModel.DataAnnotations;

namespace TestProjectDennemeyer.Data.Entities;

public class Proposal
{
    public int Id { get; set; }
    public DateTime CreatedDate { get; set; }
    [MaxLength(250)]
    public string? Comment { get; set; }
    public int CreatorId { get; set; }
    public User Creator { get; set; }
    public int ItemId { get; set; }
    public Item Item { get; set; }
    public bool Closed { get; set; } = false;
    public ICollection<ProposalParty> ProposalParties { get; set; } = new List<ProposalParty>();
}