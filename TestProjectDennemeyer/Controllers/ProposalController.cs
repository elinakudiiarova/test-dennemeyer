using Microsoft.AspNetCore.Mvc;
using TestProjectDennemeyer.Controllers.DTO;
using TestProjectDennemeyer.Controllers.Mapper;
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
    [ProducesResponseType(typeof(List<ProposalHistoryResponse>), StatusCodes.Status200OK)]
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

    /// <summary>
    /// Creates a new proposal for a shared item. You can use percentages or amounts for share.
    /// </summary>
    /// <param name="userId">The ID of the user that will create proposal.</param>
    /// <param name="proposalRequest">A body request for proposal.</param>
    /// <returns>Created proposal.</returns>
    /// <response code="201">Returns created proposal</response>
    /// <response code="404">If item does not exist</response>
    /// <response code="401">If user is not in the system</response>
    /// <response code="400">If userId is null or body is incorrect or if proposal already exists</response>
    [HttpPost]
    [ProducesResponseType(typeof(ProposalInfo), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateProposal([FromQuery] int userId, [FromBody] CreateProposalRequest proposalRequest)
    {
        if (userId <= 0)
        {
            return BadRequest("User ID must be greater than 0.");
        }
        
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        
        var user = await _userService.GetUserByIdAsync(userId);
        var item = await _itemService.GetItemByIdAsync(proposalRequest.ItemId);
        if (item is null)
        {
            return NotFound("Item does not exist.");
        }
        
        var createdProposal = await _proposalService.CreateProposalAsync(proposalRequest, user, item);
        var proposalResponse = _proposalMapper.ToProposalInfo(createdProposal, user.PartyId);

        return StatusCode(StatusCodes.Status201Created, proposalResponse);
    }

    /// <summary>
    /// Creates a counter-proposal for a shared item. You can use percentages or amounts for share.
    /// </summary>
    /// <param name="userId">The ID of the user that will create counter-proposal.</param>
    /// <param name="proposalId">The ID of proposal that being responded to.</param>
    /// <param name="proposalRequest">A body request for counter-proposal.</param>
    /// <returns>Created counter-proposal.</returns>
    /// <response code="201">Returns created counter-proposal</response>
    /// <response code="404">If item does not exist</response>
    /// <response code="401">If user is not in the system</response>
    /// <response code="400">If userId is null or body is incorrect. If there are no proposals or it is closed. If you try to create on your proposal counter-proposal.</response>
    [HttpPost("{proposalId}")]
    [ProducesResponseType(typeof(ProposalInfo), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateCounterProposal(int proposalId, [FromQuery] int userId,
        [FromBody] CreateCounterProposalRequest proposalRequest)
    {
        if (userId <= 0 || proposalId <= 0)
        {
            return BadRequest("User ID and Proposal ID must be greater than 0.");
        }
        
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        
        var user = await _userService.GetUserByIdAsync(userId);
        var item = await _itemService.GetItemByIdAsync(proposalRequest.ItemId);
        if (item is null)
        {
            return NotFound("Item does not exist.");
        }
        
        var createdProposal = await _proposalService.CreateCounterProposalAsync(proposalRequest, user, item, proposalId);
        var proposalResponse = _proposalMapper.ToProposalInfo(createdProposal, user.PartyId);
        
        return StatusCode(StatusCodes.Status201Created, proposalResponse);
    }
    
    // As a user, I want to accept an existing proposal, so I can express my wish to finalize the
    //     agreement.
    // - As a user, I want to reject an existing proposal and create a counter-proposal in one
    //     action, so I can indicate disagreement while proposing a new solution.
    // - Acceptance criteria
    // o Can’t accept my own proposal
    //     o Can’t reject my own proposal
    //     o Can’t reject proposal without providing counter-proposal
}