using CadastroCartsys.Crosscutting.Notifications;

namespace CadastroCartsys.Crosscutting.Results;

public class EnvelopResult
{
    public bool Success { get; set; }
    public IEnumerable<string> Errors { get; set; }

    public static EnvelopResult Ok() => new()
    {
        Success = true
    };

    public static EnvelopResult Fail(IEnumerable<Notification> notifications)
        => new()
        {
            Errors = notifications.Select(x=> x.Value),
            Success = false
        };
}