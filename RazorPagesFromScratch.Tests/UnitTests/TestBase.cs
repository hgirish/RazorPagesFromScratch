using RazorPagesFromScratch.Models;
using Microsoft.Extensions.DependencyInjection;
namespace RazorPagesFromScratch.Tests.UnitTests
{
    public class TestBase : TestFixture<UnitTestStartup>
    {
        protected AppDbContext db;
        
        public TestBase()
        {
            db = Server.Host.Services.GetRequiredService<AppDbContext>();
        }
    }

}
