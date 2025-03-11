namespace TestProjectDennemeyer.Controllers.DTO;

public class ProposalHistoryResponse
{
    public ItemInfo Item { get; set; } = null!;
    public List<ProposalInfo>? Proposals { get; set; }
}