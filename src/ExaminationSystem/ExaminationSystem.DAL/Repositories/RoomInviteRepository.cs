using ExaminationSystem.DAL.Entities;
using ExaminationSystem.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ExaminationSystem.DAL.Repositories;

internal class RoomInviteRepository : GenericRepository<RoomInviteDbEntry>, IRoomInviteRepository
{
    public RoomInviteRepository(DbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<RoomInviteDbEntry>> GetReceivedInvites(string userId, bool asTracked = false)
    {
        var query = asTracked ? Table.AsTracking() : Table.AsNoTracking();

        return await query.Where(i => i.ToId == userId)
            .Include(e => e.Room)
            .Include(e => e.From)
            .Include(e => e.To)
            .ToListAsync();
    }
}