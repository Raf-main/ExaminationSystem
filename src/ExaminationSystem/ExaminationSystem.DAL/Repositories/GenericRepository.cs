using System.Linq.Expressions;
using ExaminationSystem.DAL.Entities.Abstractions;
using ExaminationSystem.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ExaminationSystem.DAL.Repositories;

public abstract class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : BaseEntity, new()
{
    protected readonly DbContext Context;
    protected readonly DbSet<TEntity> Table;

    protected GenericRepository(DbContext context)
    {
        Context = context;
        Table = context.Set<TEntity>();
    }

    public virtual IQueryable<TEntity> AsQueryable(bool asTracked = false)
    {
        return asTracked ? Table.AsQueryable().AsTracking() : Table.AsQueryable().AsNoTracking();
    }

    public virtual async Task<IEnumerable<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>>? filter = null,
        bool asTracked = false, IEnumerable<Expression<Func<TEntity, object>>>? includes = null)
    {
        var query = asTracked ? Table : Table.AsNoTracking();

        if (filter != null)
        {
            query = query.Where(filter);
        }

        if (includes != null)
        {
            query = includes.Aggregate(query,
                (current, include) => current.Include(include));
        }

        return await query.ToListAsync();
    }

    public virtual async Task<TEntity?> GetByIdAsync(int id, bool asTracked = false,
        IEnumerable<Expression<Func<TEntity, object>>>? includes = null)
    {
        var query = asTracked ? Table : Table.AsNoTracking();

        if (includes != null)
        {
            query = includes.Aggregate(query,
                (current, include) => current.Include(include));
        }

        return await query.FirstOrDefaultAsync(entity => entity.Id == id);
    }

    public virtual async Task CreateAsync(TEntity entity)
    {
        await Table.AddAsync(entity);
    }

    public virtual void Update(TEntity entity)
    {
        Table.Update(entity);
    }

    public virtual async Task DeleteByIdAsync(int id)
    {
        var entity = await Table.Where(entity => entity.Id == id).FirstOrDefaultAsync();

        if (entity != null)
        {
            Table.Remove(entity);
        }
    }

    public virtual void Delete(TEntity entity)
    {
        Table.Remove(entity);
    }
}