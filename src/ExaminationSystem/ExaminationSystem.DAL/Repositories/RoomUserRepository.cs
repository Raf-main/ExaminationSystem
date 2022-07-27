using ExaminationSystem.DAL.Entities;
using ExaminationSystem.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ExaminationSystem.DAL.Repositories;

internal class RoomUserRepository : IRoomUserRepository
{

    protected readonly DbContext Context;
    protected readonly DbSet<RoomUserDbEntry> Table;

    public RoomUserRepository(DbContext context)
    {
        Context = context;
        Table = context.Set<RoomUserDbEntry>();
    }

    public IQueryable<RoomUserDbEntry> AsQueryable(bool asTracked)
    {
        return asTracked ? Table.AsTracking() : Table.AsNoTracking();
    }

    public async Task<RoomUserDbEntry?> GetRoomUserAsync(string userId, int roomId, bool asTracked = false)
    {
        return await AsQueryable(asTracked)
            .FirstOrDefaultAsync(e => e.UserId == userId && e.RoomId == roomId);
    }

    public IEnumerable<RoomUserDbEntry> GetAllRoomUsers(int roomId, bool asTracked = false)
    {
        return AsQueryable(asTracked)
            .Where(e => e.RoomId == roomId);
    }

    public async Task AddRoomUserAsync(RoomUserDbEntry entity)
    {
        await Table.AddAsync(entity);
    }

    public void Delete(RoomUserDbEntry entity)
    {
        Table.Remove(entity);
    }
}