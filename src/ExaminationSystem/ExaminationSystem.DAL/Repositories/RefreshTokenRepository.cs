using System.Linq.Expressions;
using ExaminationSystem.DAL.Entities;
using ExaminationSystem.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ExaminationSystem.DAL.Repositories;

internal class RefreshTokenRepository : GenericRepository<RefreshTokenDbEntry>, IRefreshTokenRepository
{
    public RefreshTokenRepository(DbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<RefreshTokenDbEntry>> GetByUserIdAsync(string userId)
    {
        var query = Table.Where(t => t.UserId == userId);

        return await query.ToListAsync();
    }

    public async Task DeleteRangeAsync(Expression<Func<RefreshTokenDbEntry, bool>> predicate)
    {
        var tokens = await Table.Where(predicate).ToListAsync();

        Table.RemoveRange(tokens);
    }
}