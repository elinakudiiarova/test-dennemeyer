using System.ComponentModel.DataAnnotations;

namespace TestProjectDennemeyer.Data.Entities;

public class Party
{
    public int Id { get; set; }
    [MaxLength(150)]
    public string Name { get; set; } = String.Empty;
    public DateTime CreationDate { get; set; }
    public ICollection<User> Users { get; set; } = new List<User>();
    public ICollection<Item> Items { get; set; } = new List<Item>();
    public ICollection<ProposalParty> ProposalParties { get; set; } = new List<ProposalParty>();
}