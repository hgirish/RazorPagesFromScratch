using FluentAssertions;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using RazorPagesFromScratch.Models;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace RazorPagesFromScratch.Tests.UnitTests
{
    [Trait("Category", "Unit")]
    public class HomePageTests : IClassFixture<TestFixture<UnitTestStartup>>
    {
        private readonly HttpClient _client;
        private AppDbContext _db;
        TestServer _server;
        public HomePageTests(TestFixture<UnitTestStartup> fixture)
        {
            _client = fixture.Client;
            _server = fixture.Server;
            _db = _server.Host.Services.GetRequiredService<AppDbContext>();
        }
        [Fact]
        public async Task ReturnsCorrectHtml()
        {
            var response = await _client.GetAsync("/");
            var html = await response.Content.ReadAsStringAsync();
            Assert.StartsWith("<html>", html);
            Assert.Contains("<title>To-Do lists</title>", html);
            Assert.EndsWith("</html>", html);

        }
        

        [Fact]
        public async Task DisplayAllItems()
        {
            var correctList = new TodoList();
            _db.TodoLists.Add(correctList);
            var otherList = new TodoList();
            _db.TodoLists.Add(otherList);
            //_db.SaveChanges();
            
            _db.Items.Add(new Item { Text = "itemey 1",List = correctList });
            _db.Items.Add(new Item { Text = "itemey 2", List = correctList });
            _db.Items.Add(new Item { Text = "Other list item 1", List = otherList });
            _db.Items.Add(new Item { Text = "Other list item 2", List = otherList });
            _db.SaveChanges();
            System.Console.WriteLine("correctList.id: {0}", correctList.Id);
            var response = await _client.GetAsync($"/lists/{correctList.Id}");
            response.EnsureSuccessStatusCode();
            var html = await response.Content.ReadAsStringAsync();
            html.Should().Contain("itemey 1");
            html.Should().Contain("itemey 2");
            html.Should().NotContain("Other list item");


        }
    }
}
