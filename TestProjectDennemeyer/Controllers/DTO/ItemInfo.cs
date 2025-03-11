namespace TestProjectDennemeyer.Controllers.DTO;

public class ItemInfo
{
    public string Name { get; set; } = String.Empty;
    public decimal Value { get; set; }
    public DateTime CreationDate { get; set; }
    public PartyInfo Owner { get; set; } = null!;
}