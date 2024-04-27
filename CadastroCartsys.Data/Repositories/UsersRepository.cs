using CadastroCartsys.Data.Persistence;
using CadastroCartsys.Domain.Entities;
using CadastroCartsys.Domain.Users;
using Microsoft.EntityFrameworkCore;

namespace CadastroCartsys.Data.Repositories;

public class UsersRepository: BaseRepository<User, Guid>, IUsersRepository
{
    private readonly DataContext _dataContext;

    public UsersRepository(DataContext dataContext) : base(dataContext)
    {
        _dataContext = dataContext;
    }

    public async Task<User?> GetUser(Guid id)
    {
        var user = await _dataContext.Users.FindAsync(id);
        return user ??  null;
    }

    public Task<User> FindByEmailAsync(string email) =>
        _dataContext.Set<User>().FirstOrDefaultAsync(x => x.Email == email);


}