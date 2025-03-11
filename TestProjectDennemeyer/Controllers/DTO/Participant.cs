namespace TestProjectDennemeyer.Controllers.DTO;

public class Participant
{
    public ProposalStatusEnum? DecisionStatus { get; set; }
    public decimal PurposedAmount { get; set; }
    public decimal? PurposedPercentage { get; set; }
    public PartyWithUser Party { get; set; } = null!;
}