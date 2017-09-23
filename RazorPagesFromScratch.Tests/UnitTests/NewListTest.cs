using Microsoft.AspNetCore.TestHost;
using RazorPagesFromScratch.Models;
using System.Collections.Generic;
using System.Net.Http;
using Xunit;
using Microsoft.Extensions.DependencyInjection;
using FluentAssertions;
using System.Linq;
using System.Threading.Tasks;
using RazorPagesFromScratch.Tests.Extensions;

namespace RazorPagesFromScratch.Tests.UnitTests
{
    [Trait("Category","Unit")]
    [Trait("Name","NewList")]
   public class NewListTest : TestBase
    {
        private readonly HttpClient _client;
        private AppDbContext _db;
        TestServer _server;
        public NewListTest()
        {
            _client = Client;
            _server = Server;
            _db = _server.Host.Services.GetRequiredService<AppDbContext>();
        }

        [Fact]
        public async Task CanSaveAPostRequest()
        {
            var correctList = SeedTodoList();
            var response = await PostTextAsync(correctList, "A new list item", $"/lists/{correctList.Id}/");

            var redirectLocation = response.Headers.Location.ToString();
            var items = _db.Items;
            items.Count().Should().Be(1);
            var new_item = items.First();
            new_item.Text.Should().Be("A new list item");

            
            response = await _client.GetAsync(redirectLocation); // this returns cookies in response
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            Assert.Contains("A new list item", responseString);

        }
        [Fact]
        public async Task RedirectsAfterPOSTAsync()
        {
            var correctList = SeedTodoList();
            var response = await PostTextAsync(correctList,"A new list item", $"/lists/{correctList.Id}/");
            
            response.AssertRedircts( $"/Lists/{correctList.Id}");

        }
        [Fact]
        public async Task PassesCorrectListToTemplateAsync()
        {
            var correctList = SeedTodoList();
            var response = await Client.GetAsync($"/Lists/{correctList.Id}/");
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            responseString.Should().Contain($"action=\"/Lists/{correctList.Id}");
        }
        [Fact]
        public async Task test_invalid_list_items_arent_savedAsync()
        {
            var response = await _client.GetAsync("/"); // this returns cookies in response
            response.EnsureSuccessStatusCode();
            string antiForgeryToken = await AntiForgeryHelper.ExtractAntiForgeryToken(response);
            var formPostBodyData = new Dictionary<string, string>
            {
                { "__RequestVerificationToken", antiForgeryToken}, // Add token        
                { "Item.Text", ""}
            };
            // Copy cookies from response
            var requestMessage = PostRequestHelper.CreateWithCookiesFromResponse(
                $"/lists/new", formPostBodyData, response);
            response = await _client.SendAsync(requestMessage);
            db.TodoLists.Count().Should().Be(0);
            db.Items.Count().Should().Be(0);
        }

        //private async Task<HttpResponseMessage> PostTextAsync(string text, int listId)
        //{
        //    var response = await _client.GetAsync("/"); // this returns cookies in response
        //    response.EnsureSuccessStatusCode();
        //    string antiForgeryToken = await AntiForgeryHelper.ExtractAntiForgeryToken(response);
        //    var formPostBodyData = new Dictionary<string, string>
        //    {
        //        { "__RequestVerificationToken", antiForgeryToken}, // Add token        
        //        { "Item.Text", text}
        //    };
        //    // Copy cookies from response
        //    var requestMessage = PostRequestHelper.CreateWithCookiesFromResponse(
        //        $"/lists/{listId}/AddItem", formPostBodyData, response);
        //    response = await _client.SendAsync(requestMessage);
        //    return response;
        //}
      
    }
}
