using ExaminationSystem.DAL.Entities;

namespace ExaminationSystem.DAL.Repositories.Interfaces;

public interface IRoomMessageRepository
{
    public IQueryable<RoomMessageDbEntry> AsQueryable(bool asTracked = false);

    public Task<RoomMessageDbEntry?> GetRoomMessageAsync(string userId, int roomId, int messageId,
        bool asTracked = false);

    public IEnumerable<RoomMessageDbEntry> GetAllRoomUserMessages(string userId, int roomId, bool asTracked = false);
    public IEnumerable<RoomMessageDbEntry> GetAllRoomMessages(int roomId, bool asTracked = false);
    public Task AddRoomMessageAsync(RoomMessageDbEntry entity);
}