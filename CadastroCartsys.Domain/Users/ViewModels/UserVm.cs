namespace CadastroCartsys.Domain.Users.ViewModels;

public class UserVm
{
    public Guid Id { get; set; }
    public string Email { get; set; }
    public string Name { get; set; }
    public bool Active { get; set; }
    public DateTime CreatedAt { get; set; }
}