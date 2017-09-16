using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using RazorPagesFromScratch.Models;
using System;

namespace RazorPagesFromScratch.Tests.IntegrationTests
{
    public class IntegrationTestStartup : Startup
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
            services.AddDbContext<AppDbContext>(options => options.UseInMemoryDatabase("IntegrationTestDatabase"));
        }
        public override void EnsureDatabaseCreated(AppDbContext dbContext)
        {
            // Do nothing

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
