using System.Linq.Expressions;
using ExaminationSystem.DAL.Entities.Abstractions;

namespace ExaminationSystem.DAL.Repositories.Interfaces;

public interface IGenericRepository<TEntity> where TEntity : BaseEntity, new()
{
    IQueryable<TEntity> AsQueryable(bool asTracked = false);

    Task<IEnumerable<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>>? filter = null,
        bool asTracked = false, IEnumerable<Expression<Func<TEntity, object>>>? includes = null);

    Task<TEntity?> GetByIdAsync(int id, bool asTracked = false,
        IEnumerable<Expression<Func<TEntity, object>>>? includes = null);

    Task CreateAsync(TEntity entity);
    void Update(TEntity entity);
    Task DeleteByIdAsync(int id);
    void Delete(TEntity entity);
}