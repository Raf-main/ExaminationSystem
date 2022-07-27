using System.Security.Claims;
using AutoMapper;
using ExaminationSystem.API.DTOs.Response;
using ExaminationSystem.BLL.Exceptions;
using ExaminationSystem.BLL.Managers.AccountManagers;
using ExaminationSystem.BLL.Managers.RoomManagers;
using ExaminationSystem.BLL.Models.Create;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace ExaminationSystem.API.Hubs;

[Authorize]
public class RoomChatHub : Hub
{
    private readonly IAccountManager _accountManager;
    private readonly ILogger<RoomChatHub> _logger;
    private readonly IMapper _mapper;
    private readonly IRoomMessageManager _messageManager;
    private readonly IRoomManager _roomManager;
    private readonly IRoomMessageManager _roomMessageManager;

    public RoomChatHub(IRoomMessageManager roomMessageManager, IRoomManager roomManager,
        IRoomMessageManager messageManager, IAccountManager accountManager, ILogger<RoomChatHub> logger, IMapper mapper)
    {
        _roomMessageManager = roomMessageManager;
        _roomManager = roomManager;
        _messageManager = messageManager;
        _accountManager = accountManager;
        _logger = logger;
        _mapper = mapper;
    }

    public async Task Join(int roomId)
    {
        var userId = _accountManager.GetCurrentUserClaim(ClaimTypes.NameIdentifier)!;

        if (!_roomManager.IsRoomMember(userId, roomId))
        {
            throw new PermissionDeniedException();
        }


        await Groups.AddToGroupAsync(Context.ConnectionId, roomId.ToString());
    }

    public async Task SendGroupMessage(string message, int roomId)
    {
        var userId = _accountManager.GetCurrentUserClaim(ClaimTypes.NameIdentifier)!;

        try
        {
            await _messageManager.HasChattingPermissionAsync(userId, roomId);

            var messageCreateData = new RoomMessageCreateData
            {
                RoomId = roomId,
                UserId = userId
            };

            var messageResponse =
                _mapper.Map<RoomMessageResponse>(await _roomMessageManager.AddMessageAsync(messageCreateData));

            await Clients.Group(roomId.ToString())
                .SendAsync(ClientMethodKeys.OnNewMessage, messageResponse);
        }
        catch (NotFoundException ex)
        {
            _logger.LogError(ex, ex.Message);

            await Clients.Caller.SendAsync(ClientMethodKeys.OnError, ex.Message);
        }
    }

    public async Task Leave(int roomId)
    {
        var userId = _accountManager.GetCurrentUserClaim(ClaimTypes.NameIdentifier)!;

        if (!_roomManager.IsRoomMember(userId, roomId))
        {
            throw new PermissionDeniedException($"User with id {userId} is not member of room {roomId}");
        }

        await Groups.RemoveFromGroupAsync(Context.ConnectionId, roomId.ToString());
    }
}