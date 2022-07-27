using ExaminationSystem.DAL.Entities;

namespace ExaminationSystem.DAL.Repositories.Interfaces;

public interface IRoomInviteRepository : IGenericRepository<RoomInviteDbEntry>
{
    Task<IEnumerable<RoomInviteDbEntry>> GetReceivedInvites(string userId, bool asTracked = false);
}