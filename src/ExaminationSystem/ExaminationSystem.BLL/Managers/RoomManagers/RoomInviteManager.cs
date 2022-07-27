using System.Linq.Expressions;
using AutoMapper;
using ExaminationSystem.BLL.Exceptions;
using ExaminationSystem.BLL.Helpers;
using ExaminationSystem.BLL.Managers.AccountManagers;
using ExaminationSystem.BLL.Models;
using ExaminationSystem.DAL.Entities;
using ExaminationSystem.DAL.Helpers;
using ExaminationSystem.DAL.Repositories.Interfaces;

namespace ExaminationSystem.BLL.Managers.RoomManagers;

internal class RoomInviteManager : IRoomInviteManager
{
    private readonly IMapper _mapper;
    private readonly IAccountManager _accountManager;
    private readonly IRoomManager _roomManager;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly IUnitOfWork _unitOfWork;

    public RoomInviteManager(IMapper mapper,
        IAccountManager accountManager,
        IUnitOfWork unitOfWork,
        IRoomManager roomManager,
        IDateTimeProvider dateTimeProvider)
    {
        _mapper = mapper;
        _accountManager = accountManager;
        _unitOfWork = unitOfWork;
        _roomManager = roomManager;
        _dateTimeProvider = dateTimeProvider;
    }

    public async Task<int> CreateInviteAsync(string fromUserId, string toUserId, int roomId)
    {
        var room = await _roomManager.GetRoomByIdAsync(roomId);

        if (room == null)
        {
            throw new NotFoundException($"Room with id {roomId} was nof found");
        }

        if (await _accountManager.FindUserByIdAsync(toUserId) == null)
        {
            throw new NotFoundException($"User with id {toUserId} was not found");
        }

        if (_roomManager.IsRoomMember(toUserId, roomId))
        {
            throw new BadRequestException($"User {toUserId} is already in a room {roomId}");
        }

        if (!await HasUserManagingPermissionAsync(fromUserId, roomId))
        {
            throw new PermissionDeniedException(
                $"User with id {fromUserId} has no permission to manage users in a room {roomId}");
        }

        var roomInvite = new RoomInviteDbEntry
        {
            CreatedOn = _dateTimeProvider.GetUtcNow(),
            FromId = fromUserId,
            ToId = toUserId,
            RoomId = roomId,
            IsAccepted = false
        };

        await _unitOfWork.Invites.CreateAsync(roomInvite);
        await _unitOfWork.SaveChangesAsync();

        return roomInvite.Id;
    }

    public async Task AcceptInviteAsync(int inviteId, string userId, int roomId)
    {
        var invite = await _unitOfWork.Invites.GetByIdAsync(inviteId, true);

        if (invite == null)
        {
            throw new NotFoundException($"Invite with id {userId} was not found");
        }

        if (invite.ToId != userId)
        {
            throw new PermissionDeniedException(
                $"User with id {userId} tried to accept invite with id {inviteId} for room with id {roomId}");
        }

        if (invite.RoomId != roomId)
        {
            // TODO: mb not permission denied?
            throw new PermissionDeniedException($"Invite with {inviteId} is not for room with id {roomId}");
        }

        if (invite.IsAccepted)
        {
            return;
        }

        invite.IsAccepted = true;
        await _roomManager.AddUserToRoomAsync(userId, invite.RoomId);
    }

    public async Task<IEnumerable<Invite>> GetReceivedInvitesAsync(string userId)
    {
        var invites = await _unitOfWork.Invites.GetReceivedInvites(userId);

        return _mapper.Map<IEnumerable<Invite>>(invites);
    }

    public async Task<Invite?> GetInviteAsync(int inviteId)
    {
        var includes =
            new Expression<Func<RoomInviteDbEntry, object>>[]
            {
                obj => obj.To,
                obj => obj.From,
                obj => obj.Room
            };

        var invite = await _unitOfWork.Invites.GetByIdAsync(inviteId, includes: includes);

        return _mapper.Map<Invite>(invite);
    }

    public async Task<bool> HasUserManagingPermissionAsync(string userId, int roomId)
    {
        var permissions = await _roomManager.GetRoomUserPermissionsAsync(userId, roomId);

        return permissions.HasFlag(RoomPermission.UserManaging);
    }
}