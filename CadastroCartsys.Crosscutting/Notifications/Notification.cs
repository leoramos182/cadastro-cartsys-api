namespace CadastroCartsys.Crosscutting.Notifications;

public class Notification
{
    public string Key { get; set; }
    public string Value { get; set; }

    public Notification(string value)
    {
        Value = value;
    }

    public Notification(string key, string value)
    {
        Key = key;
        Value = value;
    }
}