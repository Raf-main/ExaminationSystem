using ExaminationSystem.DAL.Entities;

namespace ExaminationSystem.DAL.Repositories.Interfaces;

public interface IRoomRepository : IGenericRepository<RoomDbEntry>
{
    IEnumerable<RoomDbEntry> GetByUserEmail(string email, bool asTracked = false);
    bool IsRoomMember(string userId, int roomId);
    bool IsRoomOwner(string userId, int roomId);
}