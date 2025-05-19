using System.Collections;
using Ecommerce.Application.Persistence;

namespace Ecommerce.Infrastructure.Persistence.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private Hashtable? repositories;
    private readonly EcommerceDbContext context;

    public UnitOfWork(EcommerceDbContext context){
        this.context = context;
    }
    public async Task<int> Complete()
    {
        try
        {
            return await context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            throw new Exception("Error en transaccion",ex);
        }
    }

    public void Dispose()
    {
        context.Dispose();
    }

    public IAsyncRepository<TEntity> Repository<TEntity>() where TEntity : class
    {
        if(repositories is null){
            repositories = new Hashtable();
        }
        var type = typeof(TEntity).Name;

        if(!repositories.ContainsKey(type)){
            var repositoryType = typeof(RepositoryBase<>);
            var repositoryInstance = Activator.CreateInstance(repositoryType.MakeGenericType(typeof(TEntity)),context);
            repositories.Add(type, repositoryInstance);
        }
        return (IAsyncRepository<TEntity>)repositories[type]!;
    }
}
