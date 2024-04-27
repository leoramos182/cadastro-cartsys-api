using System.ComponentModel.DataAnnotations;
using CadastroCartsys.Api;
using CadastroCartsys.Domain.Contracts;

namespace CadastroCartsys.Domain.Entities;

public class User: IBaseEntity, IAggregateRoot<Guid>
{
    [Key]
    public Guid Id { get; protected set; }
    public DateTime CreatedAt { get; protected set; }
    public bool Active { get; protected set; }
    public bool Deleted { get; protected set;  }
    public string Email { get; protected set; }
    public string Password { get; protected set; }
    public string Name { get; protected set; }

    public User(string name, string email, string password)
    {
        Id = Guid.NewGuid();
        CreatedAt = DateTime.UtcNow;
        Email = email;
        Name = name;
        Password = password;
        Active = true;
    }

    public bool Equals(IEntity<Guid>? other)
    {
        return other is User user && Id == user.Id;
    }
}