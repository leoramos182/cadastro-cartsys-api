using CadastroCartsys.Crosscutting.Exceptions;
using CadastroCartsys.Crosscutting.Messages;
using CadastroCartsys.Domain;
using CadastroCartsys.Domain.Persistence;
using Microsoft.EntityFrameworkCore;

namespace CadastroCartsys.Data.Persistence;

public class BaseRepository<T, TId> : IBaseRepository<T, TId>
    where T : class, IAggregateRoot<TId>
{
    protected readonly DbContext _dbContext;

    public BaseRepository(DbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public virtual ValueTask<T?> FindAsync(TId id) =>
        _dbContext.Set<T>().FindAsync(id);

    public async Task<T> AddAsync(T entity)
    {
        await _dbContext.Set<T>().AddAsync(entity);

        if (await _dbContext.SaveChangesAsync() > 0) return entity;

        throw new DomainException(DefaultMessages.DatabaseError);
    }
}