using System.Linq.Expressions;
using ExaminationSystem.DAL.Entities;

namespace ExaminationSystem.DAL.Repositories.Interfaces;

public interface IRefreshTokenRepository : IGenericRepository<RefreshTokenDbEntry>
{
    public Task<IEnumerable<RefreshTokenDbEntry>> GetByUserIdAsync(string userId);
    public Task DeleteRangeAsync(Expression<Func<RefreshTokenDbEntry, bool>> predicate);
}