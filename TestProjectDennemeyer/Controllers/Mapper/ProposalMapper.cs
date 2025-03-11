using TestProjectDennemeyer.Controllers.DTO;
using TestProjectDennemeyer.Data.Entities;

namespace TestProjectDennemeyer.Controllers.Mapper;

/// <summary>
/// Mapper for proposal controller
/// </summary>
public class ProposalMapper
{
    public ProposalInfo ToProposalInfo(Proposal proposal, int requestingPartyId)
    {
        return new ProposalInfo()
        {
            ProposalId = proposal.Id,
            Comment = proposal.Comment,
            CreatedDate = proposal.CreatedDate,
            Creator = ToPartyWithUser(proposal.Creator!, requestingPartyId),
            Participants = proposal.ProposalParties.Select(p => ToParticipant(p, requestingPartyId)).ToList()
        };
    }
    
    public ItemInfo ToItemInfo(Item item)
    {
        return new ItemInfo()
        {
            Name = item.Name,
            CreationDate = item.CreationDate,
            Owner = new PartyInfo(){ PartyId = item.OwnerPartyId, PartyName = item.OwnerParty.Name},
            Value = item.Value,
        };
    }

    private PartyWithUser? ToPartyWithUser(User user, int requestingPartyId)
    {
        var sameParty = user.PartyId == requestingPartyId;

        return new PartyWithUser()
        {
            Party = new PartyInfo()
            {
                PartyId = user.PartyId, 
                PartyName = user.Party.Name
            },
            User = new UserInfo()
            {
                UserId = user.Id, 
                Name = sameParty ? user.Name : null,
                Surname = sameParty ? user.Surname : null
            }
        };
    }
    
    private PartyWithUser? ToPartyWithUser(ProposalParty party, int requestingPartyId)
    {
        if (party.DecisionUser == null)
        {
            return new PartyWithUser()
            {
                Party = new PartyInfo()
                {
                    PartyId = party.PartyId,
                    PartyName = party.Party.Name
                }
            };
        }
        
        var sameParty = (party.DecisionUser?.PartyId == requestingPartyId);
        
        return new PartyWithUser()
        {
            Party = new PartyInfo()
            {
                PartyId = party.PartyId, 
                PartyName = party.Party.Name
            },
            User = new UserInfo()
            {
                UserId = party.DecisionUserId,
                Name = sameParty ? party.DecisionUser?.Name : null,
                Surname = sameParty ? party.DecisionUser?.Surname : null
            }
        };
    }
    
    private Participant ToParticipant(ProposalParty party, int requestingPartyId)
    {
        return new Participant()
        {
            PurposedAmount = party.Amount,
            PurposedPercentage = party.Percentage,
            Party = ToPartyWithUser(party, requestingPartyId)!,
            DecisionStatus = ToProposalStatusEnum(party.Accepted)
        };
    }

    private ProposalStatusEnum ToProposalStatusEnum(bool? proposalStatus)
    {
        return proposalStatus switch
        {
            true => ProposalStatusEnum.Approved,
            false => ProposalStatusEnum.Rejected,
            _ => ProposalStatusEnum.Pending
        };
    }
}