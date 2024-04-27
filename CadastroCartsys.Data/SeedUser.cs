using CadastroCartsys.Crosscutting.Utils;
using CadastroCartsys.Domain.Entities;

namespace CadastroCartsys.Data;

public class SeedUser
{
    public static void CreateUsers(DataContext context)
    {

        User user = new(
            "User Teste",
            "user@teste.com.br",
            PasswordUtils.Hash("123456")
        );

        context.Set<User>().Add(user);

        if(context.Set<User>().Any()) return;

        context.SaveChanges();
    }
}