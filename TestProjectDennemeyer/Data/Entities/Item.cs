using System.ComponentModel.DataAnnotations;

namespace TestProjectDennemeyer.Data.Entities;

public class Item
{
    public int Id { get; set; }
    [MaxLength(250)]
    public string Name { get; set; } = String.Empty;
    public decimal Value { get; set; }
    public DateTime CreationDate { get; set; } = DateTime.UtcNow;
    public Party? OwnerParty { get; set; }
    public int OwnerPartyId { get; set; }
    public bool Shared { get; set; } = false;
    public ICollection<Proposal> Proposals { get; set; } = new List<Proposal>();
}