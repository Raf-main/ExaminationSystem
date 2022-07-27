using System.Security.Claims;
using AutoMapper;
using ExaminationSystem.API.DTOs.Request;
using ExaminationSystem.API.DTOs.Response;
using ExaminationSystem.API.Hubs;
using ExaminationSystem.BLL.Exceptions;
using ExaminationSystem.BLL.Helpers;
using ExaminationSystem.BLL.Managers.AccountManagers;
using ExaminationSystem.BLL.Managers.RoomManagers;
using ExaminationSystem.BLL.Models.Create;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace ExaminationSystem.API.Controllers;

[Route("api/[controller]s")]
[ApiController]
[Authorize]
public class RoomMessageController : ControllerBase
{
    private readonly IHubContext<RoomChatHub> _hubContext;
    private readonly IRoomMessageManager _roomMessageManager;
    private readonly IRoomManager _roomManager;
    private readonly IAccountManager _accountManager;
    private readonly IMapper _mapper;
    private readonly ILogger<RoomMessageController> _logger;

    public RoomMessageController(IHubContext<RoomChatHub> hubContext,
        IRoomMessageManager roomMessageManager,
        IMapper mapper,
        IAccountManager accountManager,
        ILogger<RoomMessageController> logger,
        IRoomManager roomManager)
    {
        _hubContext = hubContext;
        _roomMessageManager = roomMessageManager;
        _mapper = mapper;
        _accountManager = accountManager;
        _logger = logger;
        _roomManager = roomManager;
    }

    [HttpGet]
    public IActionResult GetMessages([FromQuery] GenericRange<int> range, [FromQuery] int roomId)
    {
        if (range.Start > range.End || range.Start <= 0 || range.End <= 0)
        {
            return BadRequest("from and to must be more than 0 and from more than to");
        }

        var userId = _accountManager.GetCurrentUserClaim(ClaimTypes.NameIdentifier);

        if (userId == null || !_roomManager.IsRoomMember(userId, roomId))
        {
            return Forbid();
        }

        var messages = _roomMessageManager.GetMessages(roomId, range);

        return Ok(_mapper.Map<IEnumerable<RoomMessageResponse>>(messages));
    }

    [HttpPost]
    public async Task<IActionResult> CreateMessage(CreateRoomMessageRequest request)
    {
        var userId = _accountManager.GetCurrentUserClaim(ClaimTypes.NameIdentifier)!;

        try
        {
            if (!await _roomMessageManager.HasChattingPermissionAsync(userId, request.RoomId))
            {
                return Ok("User is banned"); // TODO: choose appropriate status code
            }
        }
        catch (NotFoundException ex)
        {
            _logger.LogError(ex, $"User with id {userId} is not member of room {request.RoomId}");

            return NotFound(ex.Message); // TODO: mb not suitable
        }

        var messageCreateData = _mapper.Map<RoomMessageCreateData>(request);
        messageCreateData.UserId = userId;

        var message = _mapper.Map<RoomMessageResponse>(await _roomMessageManager.AddMessageAsync(messageCreateData));
        await _hubContext.Clients.Group(request.RoomId.ToString())
            .SendAsync(ClientMethodKeys.OnNewMessage, message);

        return Ok(message);
    }
}