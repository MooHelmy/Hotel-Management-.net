using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

public class GenericRepo<TEntity>(ApplicationDbContext Context) : IGeneric<TEntity> where TEntity : class
{
    public async Task<int> CreateAsync(TEntity entity)
    {
        Context.Set<TEntity>().Add(entity);
        return await Context.SaveChangesAsync();
    }

    public async Task<int> DeleteAsync(int id)
    {
        var entity = await Context.Set<TEntity>().FindAsync(id);
        if (entity == null)
        {
            return 0;
        }
        Context.Set<TEntity>().Remove(entity);
        return await Context.SaveChangesAsync();
    }

    public async Task<IEnumerable<TEntity>> GetAllAsync(params Expression<Func<TEntity, object>>[] includes)
    {
        IQueryable<TEntity> query = Context.Set<TEntity>().AsNoTracking();

        foreach (var include in includes)
        {
            query = query.Include(include);
        }

        return await query.ToListAsync();
    }

    public async Task<TEntity?> GetByIdAsync(int id, params Expression<Func<TEntity, object>>[] includes)
    {
        IQueryable<TEntity> query = Context.Set<TEntity>();

        foreach (var include in includes)
        {
            query = query.Include(include);
        }

        var entity = await query.FirstOrDefaultAsync(e => EF.Property<int>(e, "Id") == id);
        if (entity == null)
        {
            throw new ItemNotFoundException($"item with  {id} is not found");
        }
        return entity;
    }

    public async Task<int> UpdateAsync(TEntity entity)
    {
        Context.Set<TEntity>().Update(entity);
        return await Context.SaveChangesAsync();
    }
}