using System.ComponentModel.DataAnnotations;

namespace TestProjectDennemeyer.Data.Entities;

public class Party
{
    public int Id { get; set; }
    [MaxLength(150)]
    public string Name { get; set; } = String.Empty;
    public DateTime CreationDate { get; set; }
    public ICollection<User> Users { get; set; } = [];
    public ICollection<Item> Items { get; set; } = [];
    public ICollection<ProposalParty> ProposalParties { get; set; } = [];
}