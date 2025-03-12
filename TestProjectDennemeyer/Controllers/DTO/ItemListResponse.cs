namespace TestProjectDennemeyer.Controllers.DTO;

public class ItemListResponse
{
    public List<ItemInfo> OwnedItems { get; set; } = [];
    public List<ItemInfo> SharedItems { get; set; } = [];
}