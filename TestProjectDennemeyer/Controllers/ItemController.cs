using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using TestProjectDennemeyer.Controllers.DTO;
using TestProjectDennemeyer.Controllers.Mappers;
using TestProjectDennemeyer.Services.Interfaces;

namespace TestProjectDennemeyer.Controllers;

/// <summary>
/// This controller provides actions to work with items
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class ItemController: ControllerBase
{
    private readonly IItemService _itemService;
    private readonly IUserService _userService;
    private readonly ItemMapper _itemMapper;

    public ItemController(IItemService itemService, ItemMapper itemMapper, IUserService userService)
    {
        _itemService = itemService;
        _itemMapper = itemMapper;
        _userService = userService;
    }
    
    /// <summary>
    /// Retrieves all items associated with the user, separating owned and shared items.
    /// </summary>
    /// <response code="200">Returns a list of items, optionally filtered and sorted</response>
    /// <response code="404">No items were found </response>
    /// <response code="401">If user is not in the system</response>
    /// <response code="400">If userId or itemId are null</response>
    /// <param name="userId">The ID of the user whose items should be retrieved.</param>
    /// <returns>A list of owned and shared items.</returns>
    [HttpGet("{userId}")]
    [ProducesResponseType(typeof(List<ProposalHistoryResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    
    public async Task<IActionResult> GetFilteredItemsForParty(int userId, [FromQuery] string? name, [FromQuery] DateTime? fromDate, [FromQuery] DateTime? toDate, [FromQuery] bool? shared, [FromQuery] string? sortBy)
    {
        var user = await _userService.GetUserByIdAsync(userId);
        var items = await _itemService.GetItemsForUserAsync(user.PartyId, name, fromDate, toDate, shared, sortBy);
        if (items == null || !items.Any())
        {
            return NotFound("There are no items for your party");
        }
        
        var itemListResponse = _itemMapper.ToItemListResponse(items, user.PartyId);
        
        return Ok(itemListResponse);
    }
}