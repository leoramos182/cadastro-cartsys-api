using CadastroCartsys.Data;
using Microsoft.EntityFrameworkCore;

namespace CadastroCartsys.Api;

public static class DatabaseConfig
{
    public static IApplicationBuilder AppUseMigrations(this IApplicationBuilder app)
    {
        using (var serviceScope = app.ApplicationServices.CreateScope())
        {
            var context = serviceScope.ServiceProvider.GetService<DataContext>();

            if (context == null)
                throw new Exception("Could not get injected DataContext");

            SeedUser.CreateUsers(context);

            return app;
        }
    }
}