using ExaminationSystem.DAL.Entities;

namespace ExaminationSystem.DAL.Repositories.Interfaces;

public interface IRoomUserRepository
{
    public IQueryable<RoomUserDbEntry> AsQueryable(bool asTracked = false);
    public Task<RoomUserDbEntry?> GetRoomUserAsync(string userId, int roomId, bool asTracked = false);
    public IEnumerable<RoomUserDbEntry> GetAllRoomUsers(int roomId, bool asTracked = false);
    public Task AddRoomUserAsync(RoomUserDbEntry entity);
    public void Delete(RoomUserDbEntry entity);
}