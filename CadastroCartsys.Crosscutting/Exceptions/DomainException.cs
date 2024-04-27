using CadastroCartsys.Crosscutting.Notifications;

namespace CadastroCartsys.Crosscutting.Exceptions;

public class DomainException: Exception
{
    private const string ExceptionMessage = "Domain Exception";
    public IEnumerable<string> Errors => Notifications.Select(x => x.Value);
    public IEnumerable<Notification> Notifications { get; private set; }

    public DomainException(params string[] notifications) : base(ExceptionMessage)
    {
        Notifications = notifications.Select(x => new Notification(x));
    }
}