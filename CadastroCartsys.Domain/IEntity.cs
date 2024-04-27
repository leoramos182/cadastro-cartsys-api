namespace CadastroCartsys.Api;

public interface IEntity<TId> : IEquatable<IEntity<TId>>
{
    TId Id { get; }
}