using AutoMapper;
using ExaminationSystem.BLL.Exceptions;
using ExaminationSystem.BLL.Managers.AccountManagers;
using ExaminationSystem.BLL.Models;
using ExaminationSystem.BLL.Models.Create;
using ExaminationSystem.DAL.Entities;
using ExaminationSystem.DAL.Helpers;
using ExaminationSystem.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ExaminationSystem.BLL.Managers.RoomManagers;

public class RoomManager : IRoomManager
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public RoomManager(IUnitOfWork unitOfWork, IMapper mapper, IAccountManager accountManager)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public IEnumerable<Room> GetRoomsByUserEmail(string email)
    {
        var rooms = _mapper.Map<IEnumerable<Room>>(_unitOfWork.Rooms.GetByUserEmail(email));

        return rooms;
    }

    public IEnumerable<Room> GetOwnedRooms(string email)
    {
        var rooms = _mapper.Map<IEnumerable<Room>>(_unitOfWork.Rooms.GetByUserEmail(email));

        return rooms;
    }

    public async Task<Room?> GetRoomByIdAsync(int id)
    {
        var room = await _unitOfWork.Rooms.GetByIdAsync(id);

        return _mapper.Map<Room>(room);
    }

    public async Task<int> CreateRoomAsync(RoomCreateData roomData)
    {
        var roomDbEntry = _mapper.Map<RoomDbEntry>(roomData);

        await _unitOfWork.Rooms.CreateAsync(roomDbEntry);

        var roomUser = new RoomUserDbEntry
        {
            Room = roomDbEntry,
            UserId = roomData.OwnerId,
            Permission = RoomPermission.Admin
        };

        await _unitOfWork.RoomUsers.AddRoomUserAsync(roomUser);

        await _unitOfWork.SaveChangesAsync();

        return roomDbEntry.Id;
    }

    public bool IsRoomOwner(string userId, int roomId)
    {
        return _unitOfWork.Rooms.IsRoomOwner(userId, roomId);
    }

    public bool IsRoomMember(string userId, int roomId)
    {
        return _unitOfWork.Rooms.IsRoomMember(userId, roomId);
    }

    public async Task DeleteUserFromRoomAsync(string userIdToBeDeleted, int roomId)
    {
        var roomUser = await _unitOfWork.RoomUsers.AsQueryable()
            .FirstOrDefaultAsync(u => u.UserId == userIdToBeDeleted && u.RoomId == roomId);

        if (roomUser != null)
        {
            _unitOfWork.RoomUsers.Delete(roomUser);
        }
    }

    public async Task AddUserToRoomAsync(string userIdToBeAdded, int roomId, RoomPermission permissions = RoomPermission.DefaultUser)
    {
        var roomUserDbEntry = new RoomUserDbEntry
        {
            RoomId = roomId,
            UserId = userIdToBeAdded,
            Permission = permissions
        };

        await _unitOfWork.RoomUsers.AddRoomUserAsync(roomUserDbEntry);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task<RoomPermission> GetRoomUserPermissionsAsync(string userId, int roomId)
    {
        var roomUser = await _unitOfWork.RoomUsers.AsQueryable()
            .FirstOrDefaultAsync(u => u.RoomId == roomId && u.UserId == userId);

        if (roomUser == null)
        {
            throw new NotFoundException($"User {userId} is not member of room {roomId}");
        }

        return roomUser.Permission;
    }

    public async Task DeleteRoomAsync(int roomId)
    {
        await _unitOfWork.Rooms.DeleteByIdAsync(roomId);
    }
}