using System.Security.Claims;

namespace CadastroCartsys.Crosscutting.Results;

public class SessionUser
{
    public SessionUser(Guid id, string email, string name)
        {
            Id = id;
            Email = email;
            Name = name;
        }

        public Guid Id { get; set; }
        public string Email { get; set; }

        public string Name { get; set; }

        public ClaimsIdentity WriteClaimsIdentity()
        {
            ClaimsIdentity identity = new(
                   new List<Claim>
                   {
                    new(CustomClaims.Id, Id.ToString()),
                    new(CustomClaims.Email, Email),
                    new(CustomClaims.Name, Name ?? "")
                   });

            return identity;
        }

        public ClaimsPrincipal WriteClaimsPrincipal()
        {
            var principal = new ClaimsPrincipal(new[] { WriteClaimsIdentity() });

            return principal;
        }
}