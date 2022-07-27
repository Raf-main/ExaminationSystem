using System.Collections;
using AutoMapper;
using ExaminationSystem.BLL.Helpers;
using ExaminationSystem.BLL.Models;
using ExaminationSystem.BLL.Models.Create;
using ExaminationSystem.DAL.Entities;
using ExaminationSystem.DAL.Helpers;
using ExaminationSystem.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ExaminationSystem.BLL.Managers.RoomManagers;

public class RoomMessageManager : IRoomMessageManager
{
    private readonly IMapper _mapper;
    private readonly IRoomManager _roomManager;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IDateTimeProvider _dateTimeProvider;

    public RoomMessageManager(IMapper mapper,
        IRoomManager roomManager,
        IUnitOfWork unitOfWork,
        IDateTimeProvider dateTimeProvider)
    {
        _mapper = mapper;
        _roomManager = roomManager;
        _unitOfWork = unitOfWork;
        _dateTimeProvider = dateTimeProvider;
    }

    public IEnumerable<RoomMessage> GetMessages(int roomId, GenericRange<int> range)
    {
        if (range.Start < 0 || range.End < 0 || range.Start > range.End)
        {
            throw new ArgumentOutOfRangeException(nameof(range));
        }

        IEnumerable<RoomMessageDbEntry> messages = _unitOfWork.RoomMessages
            .AsQueryable()
            .Where(m => m.RoomId == roomId)
            .Take(new Range(range.Start, range.End));

        return _mapper.Map<IEnumerable<RoomMessage>>(messages);
    }

    public async Task<RoomMessage> AddMessageAsync(RoomMessageCreateData createData)
    {
        var roomMessageDbEntry = _mapper.Map<RoomMessageDbEntry>(createData);
        roomMessageDbEntry.Message.CreatedOn = _dateTimeProvider.GetUtcNow();
        
        roomMessageDbEntry.Message.RoomMessages = new List<RoomMessageDbEntry>
        {
            roomMessageDbEntry,
        };

        await _unitOfWork.RoomMessages.AddRoomMessageAsync(roomMessageDbEntry);
        await _unitOfWork.SaveChangesAsync();

        return _mapper.Map<RoomMessage>(roomMessageDbEntry);
    }

    public async Task<bool> HasChattingPermissionAsync(string userId, int roomId)
    {
        var permissions = await _roomManager.GetRoomUserPermissionsAsync(userId, roomId);

        return permissions.HasFlag(RoomPermission.Chatting);
    }
}