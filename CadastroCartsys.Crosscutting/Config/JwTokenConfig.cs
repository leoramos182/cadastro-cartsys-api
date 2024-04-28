namespace CadastroCartsys.Crosscutting.Config;

public class JwTokenConfig
{
    public string Issuer { get; set; } = "CadastroCartsys";
    public string Audience { get; set; } = "CadastroCartsysUsers";
    public int ExpiresIn { get; set; }
}