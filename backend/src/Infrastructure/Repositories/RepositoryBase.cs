using System.Linq.Expressions;
using Ecommerce.Application.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Infrastructure.Persistence.Repositories;

public class RepositoryBase<T> : IAsyncRepository<T> where T : class
{
    protected readonly EcommerceDbContext context;

    public RepositoryBase(EcommerceDbContext context){
        this.context = context;
    }
    public async Task<T> AddAsync(T entity)
    {
        context.Set<T>().Add(entity);
        await context.SaveChangesAsync();
        return entity;
    }

    public void AddEntity(T entity)
    {
        context.Set<T>().Add(entity);
    }

    public void AddRange(List<T> entities)
    {
        context.Set<T>().AddRange(entities);
    }

    public async Task DeleteAsync(T entity)
    {
        context.Set<T>().Remove(entity);        
        await context.SaveChangesAsync();
    }

    public void DeleteEntity(T entity)
    {
        context.Set<T>().Remove(entity);   
    }

    public void DeleteRange(IReadOnlyList<T> entities)
    {
        context.Set<T>().RemoveRange(entities);
    }

    public async Task<IReadOnlyList<T>> GetAllAsync()
    {
        return await context.Set<T>().ToListAsync();
    }

    public async Task<IReadOnlyList<T>> GetAsync(Expression<Func<T, bool>> predicate)
    {
        return await context.Set<T>().Where(predicate).ToListAsync();
    }

    public async Task<IReadOnlyList<T>> GetAsync(Expression<Func<T, bool>>? predicate, Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy, string? includeString, bool disableTracking = true)
    {
        IQueryable<T> query = context.Set<T>();
        if(disableTracking) query = query.AsNoTracking();
        if(!string.IsNullOrWhiteSpace(includeString)) query = query.Include(includeString);
        if(predicate != null) query = query.Where(predicate);
        if(orderBy != null)
            return await orderBy(query).ToListAsync();

        return await query.ToListAsync();
    }

    public async Task<IReadOnlyList<T>> GetAsync(Expression<Func<T, bool>>? predicate, Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null, List<Expression<Func<T, object>>>? includes = null, bool disableTracking = true)
    {
        IQueryable<T> query = context.Set<T>();
        if(disableTracking) query = query.AsNoTracking();
        if(includes != null) query = includes.Aggregate(query, (current, include)=> current.Include(include));
        if(predicate != null) query = query.Where(predicate);
        if(orderBy != null)
            return await orderBy(query).ToListAsync();

        return await query.ToListAsync();
    }

    public async Task<T> GetByIdAsync(int id)
    {
        return (await context.Set<T>().FindAsync(id))!;
    }

    public async Task<T> GetEntityAsync(Expression<Func<T, bool>>? predicate, List<Expression<Func<T, object>>>? includes = null, bool disableTracking = true)
    {
        IQueryable<T> query = context.Set<T>();
        if(disableTracking) query = query.AsNoTracking();
        if(includes != null) query = includes.Aggregate(query, (current, include)=> current.Include(include));
        if(predicate != null) query = query.Where(predicate);
        return (await query.FirstOrDefaultAsync())!;
    }

    public async Task<T> UpdateAsync(T entity)
    {
        context.Set<T>().Attach(entity);
        context.Entry(entity).State = EntityState.Modified;
        await context.SaveChangesAsync();
        return entity;
    }

    public void UpdateEntity(T entity)
    {
        context.Set<T>().Attach(entity);
        context.Entry(entity).State = EntityState.Modified;
    }
}