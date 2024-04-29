using System.Linq.Expressions;
using CadastroCartsys.Crosscutting.Utils;
using CadastroCartsys.Data.Persistence;
using CadastroCartsys.Domain.Entities;
using CadastroCartsys.Domain.Users;
using CadastroCartsys.Domain.Users.Filters;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

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

    public async Task<List<User>> GetAllUsers()
    {
        var users =  _dataContext.Users.ToList();
        return users;
    }

    public Task<User> FindByEmailAsync(string email) =>
        _dataContext.Set<User>().FirstOrDefaultAsync(x => x.Email == email);

    public async Task<List<User>> Filter(SearchUsersFilters filter)
    {
        var predicate = PredicateBuilder.True<User>();

        if (!string.IsNullOrEmpty(filter.Name))
        {
            var pattern = "%" + filter.Name + "%";
            predicate = predicate.And(u => EF.Functions.Like(u.Name, pattern));
        }

        if (!string.IsNullOrEmpty(filter.Email))
        {
            var pattern = "%" + filter.Email + "%";
            predicate = predicate.And(u => EF.Functions.Like(u.Email, pattern));
        }

        if (filter.Active != null)
        {
            predicate = predicate.And(u => u.Active == filter.Active.Value);
        }

        var filteredUsers = _dataContext.Set<User>().Where(predicate).ToList();

        return filteredUsers;

    }
}