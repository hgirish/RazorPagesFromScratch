using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using RazorPagesFromScratch.Models;

namespace RazorPagesFromScratch
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            //services.AddDbContext<AppDbContext>(options =>
            //{
            //    options.UseInMemoryDatabase("superlist-inmemory");
            //});
            SetUpDatabase(services);
            services.AddMvc()
                .AddRazorPagesOptions(
                options =>
                {
                    options.RootDirectory = "/Pages";
                    options.Conventions.AddPageRoute("/Lists/AddItem", "Lists/{id:int}/AddItem");
                });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            // within your Configure method:
            using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>()
              .CreateScope())
            {
                var dbContext = serviceScope.ServiceProvider.GetService<AppDbContext>();
                EnsureDatabaseCreated(dbContext);
            }
            app.UseStaticFiles();
            app.UseMvc();
        }
        public virtual void SetUpDatabase(IServiceCollection services)
        {
            var connectionStringBuilder = new SqliteConnectionStringBuilder
            {
                DataSource = "Superlists.sqlite3"
            };
            var connectionString = connectionStringBuilder.ToString();
            var connection = new SqliteConnection(connectionString);

            services.AddDbContext<AppDbContext>(
                options => options.UseSqlite(connection));

            //services.AddDbContext<AppDbContext>(options =>
            //{
            //    options.UseInMemoryDatabase("superlist-inmemory");
            //});
        }

        public virtual void EnsureDatabaseCreated(AppDbContext dbContext)
        {
            dbContext.Database.OpenConnection();
            dbContext.Database.EnsureCreated();
            try
            {
                dbContext.Database.Migrate();

            }
            catch (System.Exception ex)
            {

                System.Console.WriteLine(ex.Message);
            }

        }
    }
}
