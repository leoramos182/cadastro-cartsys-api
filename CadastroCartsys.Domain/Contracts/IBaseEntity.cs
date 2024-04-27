namespace CadastroCartsys.Domain.Contracts;

public interface IBaseEntity
{
    Guid Id { get; }
    DateTime CreatedAt { get; }
    bool Active { get; }
    bool Deleted { get; }
}