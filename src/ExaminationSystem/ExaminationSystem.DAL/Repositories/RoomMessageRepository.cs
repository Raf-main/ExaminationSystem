using ExaminationSystem.DAL.Entities;
using ExaminationSystem.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ExaminationSystem.DAL.Repositories;

public class RoomMessageRepository : IRoomMessageRepository
{

    protected readonly DbContext Context;
    protected readonly DbSet<RoomMessageDbEntry> Table;

    public RoomMessageRepository(DbContext context)
    {
        Context = context;
        Table = context.Set<RoomMessageDbEntry>();
    }

    public IQueryable<RoomMessageDbEntry> AsQueryable(bool asTracked = false)
    {
        return asTracked ? Table.AsTracking() : Table.AsNoTracking();
    }

    public async Task<RoomMessageDbEntry?> GetRoomMessageAsync(string userId, int roomId, int messageId,
        bool asTracked = false)
    {
        return await AsQueryable(asTracked)
            .FirstOrDefaultAsync(e => e.UserId == userId && e.RoomId == roomId && e.MessageId == messageId);
    }

    public IEnumerable<RoomMessageDbEntry> GetAllRoomUserMessages(string userId, int roomId, bool asTracked = false)
    {
        return AsQueryable(asTracked)
            .Where(e => e.UserId == userId && e.RoomId == roomId);
    }

    public IEnumerable<RoomMessageDbEntry> GetAllRoomMessages(int roomId, bool asTracked = false)
    {
        return AsQueryable(asTracked)
            .Where(e => e.RoomId == roomId);
    }

    public async Task AddRoomMessageAsync(RoomMessageDbEntry entity)
    {
        await Table.AddAsync(entity);
    }
}