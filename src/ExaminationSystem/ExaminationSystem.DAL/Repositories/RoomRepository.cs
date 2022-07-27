using ExaminationSystem.DAL.Entities;
using ExaminationSystem.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ExaminationSystem.DAL.Repositories;

internal class RoomRepository : GenericRepository<RoomDbEntry>, IRoomRepository
{
    public RoomRepository(DbContext context) : base(context)
    {
    }

    public IEnumerable<RoomDbEntry> GetByUserEmail(string email, bool asTracked = false)
    {
        var query = asTracked ? Table.AsTracking() : Table.AsNoTracking();

        return query.Where(r => r.Members.FirstOrDefault(m => m.User.Email == email) != null || r.Owner.Email == email);
    }

    public IEnumerable<RoomDbEntry> GetOwnedRooms(string email, bool asTracked = false)
    {
        var query = asTracked ? Table.AsTracking() : Table.AsNoTracking();

        return query.Where(r => r.Owner.Email == email);
    }

    public bool IsRoomMember(string userId, int roomId)
    {
        var isMember = Table.Where(r => r.Id == roomId)
            .SelectMany(m => m.Members)
            .Any(m => m.UserId == userId);

        return isMember;
    }

    public bool IsRoomOwner(string userId, int roomId)
    {
        var isOwner = Table.Any(r => r.Id == roomId && r.OwnerId == userId);

        return isOwner;
    }
}