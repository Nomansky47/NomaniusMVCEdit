using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.Web.CodeGeneration.Design;
namespace NomaniusMVC
{
    public static class DbInitializer
    {
        private static void Initialize(object database)
        {
            ((DatabaseFacade)database).EnsureCreated();
        }
        public static void CreateDbIfNotExists<T>(IHost host)
        {
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    var context = services.GetRequiredService<T>();
                    Initialize((context as DbContext).Database);
                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "An error occurred creating the DB.");
                }
            }
        }
        public static void AddSqlServerContext<T>(IServiceCollection thisServices, string connectionString) where T : DbContext
        {
            thisServices.AddDbContext<T>(options => options.UseSqlServer(connectionString));
        }

        public static void AddMySqlContext<T>(IServiceCollection thisServices, string connectionString) where T : DbContext
        {
            thisServices.AddDbContextPool<T>(options => options.UseMySql(connectionString,ServerVersion.AutoDetect(connectionString)));
        }
        public static void AddPostgresContext<T>(IServiceCollection thisServices, string connectionString) where T : DbContext
        {
            thisServices.AddDbContext<T>(options => options.UseNpgsql(connectionString));
        }
    }
}
