using System.Security.Claims;
using AutoMapper;
using ExaminationSystem.API.DTOs.Request;
using ExaminationSystem.API.DTOs.Response;
using ExaminationSystem.BLL.Exceptions;
using ExaminationSystem.BLL.Managers.AccountManagers;
using ExaminationSystem.BLL.Managers.RoomManagers;
using ExaminationSystem.BLL.Models.Create;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ExaminationSystem.API.Controllers;

[Route("api/[controller]s")]
[ApiController]
[Authorize]
public class RoomController : ControllerBase
{
    private readonly IAccountManager _accountManager;
    private readonly IMapper _mapper;
    private readonly IRoomManager _roomManager;
    private readonly IRoomInviteManager _roomInviteManager;
    private readonly ILogger<RoomController> _logger;

    public RoomController(IAccountManager accountManager,
        IRoomManager roomManager,
        IMapper mapper,
        IRoomInviteManager roomInviteManager, 
        ILogger<RoomController> logger)
    {
        _accountManager = accountManager;
        _roomManager = roomManager;
        _mapper = mapper;
        _roomInviteManager = roomInviteManager;
        _logger = logger;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<RoomResponse>))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public IActionResult GetRooms()
    {
        var email = _accountManager.GetCurrentUserClaim(ClaimTypes.Email);

        if (string.IsNullOrEmpty(email))
        {
            return Unauthorized();
        }

        var rooms = _roomManager.GetRoomsByUserEmail(email);

        return Ok(_mapper.Map<IEnumerable<RoomResponse>>(rooms));
    }

    [HttpGet("{roomId:int}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(RoomResponse))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetRoom(int roomId)
    {
        var currentUserId = _accountManager.GetCurrentUserClaim(ClaimTypes.NameIdentifier);

        if (string.IsNullOrEmpty(currentUserId))
        {
            return Unauthorized(roomId);
        }

        var room = await _roomManager.GetRoomByIdAsync(roomId);

        if (room == null)
        {
            return NotFound($"Room with id {roomId} doesn't exist");
        }

        if (room.OwnerId != currentUserId)
        {
            Forbid("Permission denied");
        }

        return Ok(_mapper.Map<RoomResponse>(room));
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(RoomResponse))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> CreateRoom([FromBody] CreateRoomRequest createRoomRequest)
    {
        var email = _accountManager.GetCurrentUserClaim(ClaimTypes.Email);

        if (string.IsNullOrEmpty(email))
        {
            return Unauthorized();
        }

        var user = await _accountManager.FindUserByEmailAsync(email);

        if (user == null)
        {
            return Unauthorized();
        }

        var roomCreateData = _mapper.Map<RoomCreateData>(createRoomRequest);
        roomCreateData.OwnerId = user.Id;

        var roomId = await _roomManager.CreateRoomAsync(roomCreateData);

        return Ok(new { roomId });
    }

    [HttpGet("Invites/Received")]
    public async Task<IActionResult> GetReceivedRoomInvites()
    {
        var userId = _accountManager.GetCurrentUserClaim(ClaimTypes.NameIdentifier)!;
        var invites = await _roomInviteManager.GetReceivedInvitesAsync(userId);

        return Ok(_mapper.Map<IEnumerable<RoomInviteResponse>>(invites));
    }

    [HttpGet("{roomId:int}/Invites/{inviteId:int}")]
    public async Task<IActionResult> GetRoomInvite([FromRoute] int roomId, [FromRoute] int inviteId)
    {
        var userId = _accountManager.GetCurrentUserClaim(ClaimTypes.NameIdentifier)!;

        try
        {
            var invite = await _roomInviteManager.GetInviteAsync(inviteId);

            if (invite == null)
            {
                return NotFound($"Invite with id {inviteId} was not found");
            }

            if (invite.To.Id != userId || !await _roomInviteManager.HasUserManagingPermissionAsync(userId, roomId))
            {
                return Forbid();
            }

            return Ok(_mapper.Map<RoomInviteResponse>(invite));
        }
        catch (NotFoundException ex)
        {
            _logger.LogError(ex, ex.Message);

            return NotFound(ex.Message);
        }
    }

    [HttpPost("{roomId:int}/Invites/{inviteId:int}/Accept")]
    public async Task<IActionResult> AcceptRoomInvite([FromRoute] int roomId, [FromRoute] int inviteId)
    {
        var userId = _accountManager.GetCurrentUserClaim(ClaimTypes.NameIdentifier)!;

        try
        {
            await _roomInviteManager.AcceptInviteAsync(inviteId, userId, roomId);
        }
        catch (NotFoundException ex)
        {
            _logger.LogError(ex, $"Invite with id {inviteId} was not found");

            return NotFound(ex.Message);
        }
        catch (PermissionDeniedException ex)
        {
            _logger.LogError(ex,
                $"Permission denied for user with id {userId}, invite id {inviteId}, room id {roomId}");

            return Forbid();
        }

        return Ok("Invite was successfully accepted");
    }

    [HttpPost("{roomId:int}/Invites")]
    public async Task<IActionResult> CreateRoomInvite([FromRoute] int roomId,
        [FromBody] CreateRoomInviteRequest request)
    {
        var userId = _accountManager.GetCurrentUserClaim(ClaimTypes.NameIdentifier)!;

        try
        {
            var inviteId = await _roomInviteManager.CreateInviteAsync(userId, request.ToUserId, roomId);

            return Ok(new { inviteId });
        }
        catch (NotFoundException ex)
        {
            _logger.LogError(ex, ex.Message);

            return NotFound(ex.Message);
        }
        catch (PermissionDeniedException ex)
        {
            _logger.LogError(ex, $"Permission denied for user with id {userId}");

            return Forbid();
        }
    }
}