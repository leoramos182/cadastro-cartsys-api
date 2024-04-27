using CadastroCartsys.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CadastroCartsys.Data;

public class DataContext: DbContext
{
    public DbContext DbContext { get; }

    public DataContext(DbContextOptions<DataContext>options) : base(options)
    {

    }

    public DbSet<User> Users { get; set; } = null!;
}