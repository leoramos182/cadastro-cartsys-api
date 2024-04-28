namespace CadastroCartsys.Crosscutting.Models;

public class JwToken
{
    public DateTime? NotBefore { get; set; }
    public DateTime? ExpiresIn { get; set;}
    public string AccessToken { get;  set;}
}