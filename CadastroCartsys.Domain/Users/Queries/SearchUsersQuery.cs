namespace CadastroCartsys.Domain.Users.Queries;

public class SearchUsersQuery
{
    public string? Email { get; set; }
    public string? Name { get; set; }
    public bool? Active { get; set; }
}