using Microsoft.AspNetCore.Mvc;
using TestProjectDennemeyer.Controllers.DTO;
using TestProjectDennemeyer.Controllers.Mapper;
using TestProjectDennemeyer.Services;
using TestProjectDennemeyer.Services.Interfaces;

namespace TestProjectDennemeyer.Controllers;

/// <summary>
/// This controller provides actions to work with proposals
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class ProposalController : ControllerBase
{
    private readonly IProposalService _proposalService;
    private readonly ProposalMapper _proposalMapper;
    private readonly IItemService _itemService;
    private readonly IUserService _userService;
    

    public ProposalController(IProposalService proposalService, ProposalMapper proposalMapper, IItemService itemService, IUserService userService)
    {
        _proposalService = proposalService;
        _proposalMapper = proposalMapper;
        _itemService = itemService;
        _userService = userService;
    }

    /// <summary>
    /// Gets negotiation history returning a list of proposals sorted in chronological order.
    /// </summary>
    /// <remarks>
    /// Sample request:
    /// 
    ///     GET /api/1
    /// 
    /// </remarks>
    /// <param name="itemId">The ID of the item.</param>
    /// <param name="userId">The ID of the user.</param>
    /// <returns>A history of proposals connected to item</returns>
    /// <response code="200">Returns a list of proposals</response>
    /// <response code="404">If no proposals are found or item is not found or party is not in any proposals</response>
    /// <response code="401">If user is not in the system</response>
    /// <response code="400">If userId or itemId are null</response>
    [HttpGet("{itemId}")]
    [ProducesResponseType(typeof(IEnumerable<ProposalHistoryResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetProposalHistory(int itemId, [FromQuery] int userId)
    {
        if (userId <= 0 || itemId <= 0)
        {
            return BadRequest("User ID and Item ID must be greater than 0.");
        }
        
        var user = await _userService.GetUserByIdAsync(userId);
        var item = await _itemService.GetItemByIdAsync(itemId);

        if (item is null)
        {
            return NotFound("Item does not exist.");
        }

        var proposals = await _proposalService.GetAllProposalsByItemAsync(itemId, user.PartyId);
        if (proposals is null or {Count: 0})
        {
            return NotFound("Item does not have any existing proposals connected to your party.");
        }

        var proposalHistoryResponse = new ProposalHistoryResponse()
        {
            Proposals = proposals.Select(p => _proposalMapper.ToProposalInfo(p, user.PartyId)).ToList(),
            Item = _proposalMapper.ToItemInfo(item)
        };

        return Ok(proposalHistoryResponse);
    }

    [HttpPost]
    public async Task<IActionResult> CreateProposal([FromQuery] int userId, [FromBody] CreateProposalRequest proposalRequest)
    {
        //As a user, I want to create a new proposal for a shared item, so I can propose payment
        // ratios for all parties involved, in order to pay full amount together.
        // - Acceptance Criteria:
        // o Proposals can include an optional message/comment.
        // o Should be able to submit proposal either in percentages (i.e. 25%) or in amounts
        // (i.e. 25.000€)
        
        //  Submitting proposal should be allowed only if there are no other proposals.

        //  If there are other proposals, then counter proposal must be submitted
        // (handled through another story)
        
        
        //if user party has other proposals -> error that must be done contrproposal, so we can not 
        return Ok();
    }

    [HttpPost("{proposalId}")]
    public async Task<IActionResult> CreateContrProposal(int proposalId, [FromQuery] int userId,
        [FromBody] CreateProposalRequest proposalRequest)
    {
        return Ok();
    }
}