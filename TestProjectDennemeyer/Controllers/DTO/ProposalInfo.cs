using System.ComponentModel.DataAnnotations;

namespace TestProjectDennemeyer.Controllers.DTO;

public class ProposalInfo
{
    public int ProposalId { get; set; }
    public PartyWithUser? Creator { get; set; }
    [MaxLength(250)]
    public string? Comment { get; set; }
    public DateTime? CreatedDate { get; set; }
    public List<Participant> Participants { get; set; } = [];
}