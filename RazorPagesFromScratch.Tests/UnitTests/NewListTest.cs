using Microsoft.AspNetCore.TestHost;
using RazorPagesFromScratch.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using Xunit;
using Microsoft.Extensions.DependencyInjection;
using FluentAssertions;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using RazorPagesFromScratch.Tests.Extensions;

namespace RazorPagesFromScratch.Tests.UnitTests
{
    [Trait("Category","Unit")]
    [Trait("Name","NewList")]
   public class NewListTest : TestFixture<UnitTestStartup>
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
            var response = await PostTextAsync("A new list item");
            var items = _db.Items;
            items.Count().Should().Be(1);
            var new_item = items.First();
            new_item.Text.Should().Be("A new list item");

            Assert.Equal(HttpStatusCode.Redirect, response.StatusCode);
            Assert.Equal("/lists/1/", response.Headers.Location.ToString());
            response = await _client.GetAsync("/"); // this returns cookies in response
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            Assert.Contains("A new list item", responseString);

        }
        [Fact]
        public async Task RedirectsAfterPOSTAsync()
        {
            var response = await PostTextAsync("A new list item");
            var newList = _db.TodoLists.First();
            response.AssertRedircts( $"/lists/{newList.Id}/");

        }

        

        private async Task<HttpResponseMessage> PostTextAsync(string text)
        {
            var response = await _client.GetAsync("/"); // this returns cookies in response
            response.EnsureSuccessStatusCode();
            string antiForgeryToken = await AntiForgeryHelper.ExtractAntiForgeryToken(response);
            var formPostBodyData = new Dictionary<string, string>
            {
                { "__RequestVerificationToken", antiForgeryToken}, // Add token        
                { "Item.Text", text}
            };
            // Copy cookies from response
            var requestMessage = PostRequestHelper.CreateWithCookiesFromResponse(
                "/lists/new", formPostBodyData, response);
            response = await _client.SendAsync(requestMessage);
            return response;
        }
    }
}
