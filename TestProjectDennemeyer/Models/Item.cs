using System.ComponentModel.DataAnnotations;

namespace TestProjectDennemeyer.Models;

public class Item
{
    public int Id { get; set; }
    [MaxLength(250)]
    public string Name { get; set; } = String.Empty;
    public decimal Value { get; set; }
    public DateTime CreationDate { get; set; }
    public Party OwnerParty { get; set; } = null!;
    public int OwnerPartyId { get; set; } 
}