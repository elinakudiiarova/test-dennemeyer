using TestProjectDennemeyer.Controllers.DTO;
using TestProjectDennemeyer.Data.Entities;

namespace TestProjectDennemeyer.Controllers.Mappers;

/// <summary>
/// Mapper for item controller
/// </summary>
public class ItemMapper
{
    public ItemListResponse ToItemListResponse(List<Item> items, int partyId)
    {
        return new ItemListResponse()
        {
            OwnedItems = items.Where(i => i.OwnerPartyId == partyId)
                .Select(ToItemInfo)
                .ToList(),
            SharedItems = items
                .Where(i => i.Proposals.Any(p => p.Closed == true && p.ProposalParties.All(pp => pp.Accepted == true)))
                .Select(ToItemInfo)
                .ToList()
        };
    }
    
    private ItemInfo ToItemInfo(Item item)
    {
        return new ItemInfo
        {
            Name = item.Name,
            Value = item.Value,
            CreationDate = item.CreationDate,
            Owner = new PartyInfo { PartyId = item.OwnerPartyId, PartyName = item.OwnerParty.Name }
        };
    }
}