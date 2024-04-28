using System.Linq.Expressions;
using CadastroCartsys.Domain.Entities;
using CadastroCartsys.Domain.Persistence;
using CadastroCartsys.Domain.Users.Queries;

namespace CadastroCartsys.Domain.Users;

public interface IUsersRepository: IBaseRepository<User, Guid>
{
    Task<User?> GetUser(Guid id);
    Task<List<User>> GetAllUsers();
    Task<User> FindByEmailAsync(string email);
    Task<List<User>> Filter(SearchUsersQuery filter);


}