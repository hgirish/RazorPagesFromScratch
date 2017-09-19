using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using RazorPagesFromScratch.Models;
namespace RazorPagesFromScratch.Tests.UnitTests
{
    public class UnitTestStartup : Startup
    {
        public override void SetUpDatabase(IServiceCollection services)
        {
            //var connectionStringBuilder = new SqliteConnectionStringBuilder
            //{
            //    DataSource = ":memory:"
            //};
            //var connectionString = connectionStringBuilder.ToString();
            //var connection = new SqliteConnection(connectionString);

            //services.AddDbContext<AppDbContext>(
            //    options => options.UseSqlite(connection));
            //Console.WriteLine("Seupdatabase finished");
            services.AddDbContext<AppDbContext>(options => options.UseInMemoryDatabase("UnitTestDatabase"));
        }
        public override void EnsureDatabaseCreated(AppDbContext dbContext)
        {
            // Do nothing - Since we are using InMemoryDatabase


            //dbContext.Database.OpenConnection();
            //dbContext.Database.EnsureCreated();

            //try
            //{
            //    dbContext.Database.Migrate();


            //}
            //catch (Exception ex)
            //{

            //    Console.WriteLine(ex.Message);
            //}
        }
    }
}
