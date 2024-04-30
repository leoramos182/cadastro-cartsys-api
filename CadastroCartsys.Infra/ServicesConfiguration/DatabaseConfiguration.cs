using CadastroCartsys.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace CadastroCartsys.Api;

public static class DatabaseConfiguration
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