namespace TestProjectDennemeyer.Data.Entities;

public class ProposalParty
{
    public int Id { get; set; }
    public Party? Party { get; set; }
    public int PartyId { get; set; }
    public Proposal? Proposal { get; set; }
    public int ProposalId { get; set; }
    public bool? Accepted { get; set; }
    public int? DecisionUserId { get; set; }
    public User? DecisionUser { get; set; }
    public decimal Amount { get; set; }
    public decimal? Percentage { get; set; }
}