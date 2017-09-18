using Microsoft.AspNetCore.TestHost;
using RazorPagesFromScratch.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
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
