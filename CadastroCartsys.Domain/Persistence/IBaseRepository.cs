namespace CadastroCartsys.Domain.Persistence;

public interface IBaseRepository<T, TId> where T : class, IAggregateRoot<TId>
{
    Task<T> AddAsync(T entity);

}