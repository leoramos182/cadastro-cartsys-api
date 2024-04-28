using CadastroCartsys.Domain.Entities;
using CadastroCartsys.Domain.Users.ViewModels;

namespace CadastroCartsys.Domain.Projections;

public static class UserProjection
{
    public static IQueryable<UserVm> ToVm(this IQueryable<User> query) => query.Select(user => new UserVm
    {
        Id = user.Id,
        Name = user.Name,
        Email = user.Email,
        Active = user.Active,
        CreatedAt = user.CreatedAt
    });

    public static IEnumerable<UserVm> ToVm(this IEnumerable<User> query) => query.AsQueryable().ToVm();

    public static UserVm ToVm(this User user) => new[] {user}.AsQueryable().ToVm().FirstOrDefault()!;


}