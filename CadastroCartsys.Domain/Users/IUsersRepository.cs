using CadastroCartsys.Domain.Entities;
using CadastroCartsys.Domain.Persistence;

namespace CadastroCartsys.Domain.Users;

public interface IUsersRepository: IBaseRepository<User, Guid>
{
    Task<User?> GetUser(Guid id);
    Task<User> FindByEmailAsync(string email);

}