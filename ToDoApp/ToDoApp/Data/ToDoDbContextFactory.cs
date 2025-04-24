using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace ToDoApp.Data;

public class ToDoDbContextFactory : IDesignTimeDbContextFactory<ToDoDbContext>
{
    public ToDoDbContext CreateDbContext(string[] args)
    {
        var config = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();

        var optionsBuilder = new DbContextOptionsBuilder<ToDoDbContext>();
        var connectionString = config.GetConnectionString("ToDoDb");

        optionsBuilder.UseNpgsql(connectionString);

        return new ToDoDbContext(optionsBuilder.Options);
    }
}
