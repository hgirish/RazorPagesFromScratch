using RazorPagesFromScratch.Models;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using System.Net.Http;
using System.Collections.Generic;
using Xunit;

namespace RazorPagesFromScratch.Tests.UnitTests
{
    public class TestBase : TestFixture<UnitTestStartup>
    {
        protected AppDbContext db;
        
        public TestBase()
        {
            db = Server.Host.Services.GetRequiredService<AppDbContext>();
        }
        protected TodoList SeedTodoList()
        {
            var otherList = new TodoList();
            var correctList = new TodoList();
            db.TodoLists.Add(otherList);
            db.TodoLists.Add(correctList);
            db.SaveChanges();
            return correctList;
        }

        protected async Task<HttpResponseMessage> PostTextAsync(TodoList list, string text, string url)
        {
            var response = await Client.GetAsync("/"); // this returns cookies in response
            response.EnsureSuccessStatusCode();
            string antiForgeryToken = await AntiForgeryHelper.ExtractAntiForgeryToken(response);
            var formPostBodyData = new Dictionary<string, string>
            {
                { "__RequestVerificationToken", antiForgeryToken}, // Add token        
                { "Item.Text", text}
            };
            // Copy cookies from response
            var requestMessage = PostRequestHelper.CreateWithCookiesFromResponse(
                url, formPostBodyData, response);
            response = await Client.SendAsync(requestMessage);
            return response;
        }
    }

}
