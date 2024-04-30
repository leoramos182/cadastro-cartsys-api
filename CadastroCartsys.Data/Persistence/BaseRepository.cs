using CadastroCartsys.Crosscutting.Exceptions;
using CadastroCartsys.Crosscutting.Messages;
using CadastroCartsys.Crosscutting.Models;
using CadastroCartsys.Domain;
using CadastroCartsys.Domain.Contracts;
using CadastroCartsys.Domain.Persistence;
using Microsoft.EntityFrameworkCore;

namespace CadastroCartsys.Data.Persistence;

public class BaseRepository<T, TId> : IBaseRepository<T, TId>
    where T : class, IAggregateRoot<TId>, IBaseEntity
{
    protected readonly DbContext _dbContext;

    public BaseRepository(DbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public virtual ValueTask<T?> FindAsync(TId id) =>
        _dbContext.Set<T>().FindAsync(id);

    public async Task<PagedList<T>> GetPagedList(int page, int pageSize, string order)
    {

        var pages = _dbContext.Set<T>()
            .Select(t => t);
        var totalItens = pages.Count();
        var totalPages = totalItens / pageSize;
        pages = pages
            .Skip((page-1) * pageSize)
            .Take(pageSize);
        var pageItens = await (order == "ASC"
                ? pages.OrderBy(p => p.CreatedAt)
                : pages.OrderByDescending(p => p.CreatedAt)
            ).ToListAsync();

        return new PagedList<T>
        {
            Page = page,
            PageSize = pageSize,
            Order = order,
            TotalPages = totalPages,
            TotalItens = totalItens,
            Itens = pageItens
        };
    }
    public async Task<T> AddAsync(T entity)
    {
        await _dbContext.Set<T>().AddAsync(entity);

        if (await _dbContext.SaveChangesAsync() > 0) return entity;

        throw new DomainException(DefaultMessages.DatabaseError);
    }

    public T Modify(T entity)
    {
        _dbContext.Set<T>().Update(entity);

        if (SaveChanges()) return entity;

        throw new DomainException(DefaultMessages.DatabaseError);
    }

    public T Remove(T entity)
    {
        _dbContext.Set<T>().Remove(entity);

        if (SaveChanges()) return entity;

        throw new DomainException(DefaultMessages.DatabaseError);
    }

    public bool SaveChanges()
    {
        try
        {
            return _dbContext.SaveChanges() > 0;
        }
        catch
        {
            throw new DomainException(DefaultMessages.DatabaseError);
        }
    }
}