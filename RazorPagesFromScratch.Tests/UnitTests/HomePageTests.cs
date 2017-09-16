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
        public HomePageTests(TestFixture<UnitTestStartup> fixture)
        {
            _client = fixture.Client;
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
        public async Task CanSaveAPostRequest()
        {
            var response = await _client.GetAsync("/"); // this returns cookies in response
            response.EnsureSuccessStatusCode();
            string antiForgeryToken = await AntiForgeryHelper.ExtractAntiForgeryToken(response);
            var formPostBodyData = new Dictionary<string, string>
            {
                { "__RequestVerificationToken", antiForgeryToken}, // Add token        
                { "Item.Text", "A new list item"}
            };
            // Copy cookies from response
            var requestMessage = PostRequestHelper.CreateWithCookiesFromResponse("/", formPostBodyData, response);
            response = await _client.SendAsync(requestMessage);

            // response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.Redirect, response.StatusCode);
            Assert.Equal("/", response.Headers.Location.ToString());
            //string responseString = await response.Content.ReadAsStringAsync();
            //System.Console.WriteLine($"Response String: {responseString}");
            //Assert.Contains("A new list item", responseString);
            //Assert.Contains("<h1>Your To-Do list</h1>", responseString);
            //Assert.Contains("<table id=\"id_list_table\">", responseString);
            //Assert.Contains("A new list item", responseString);
            response = await _client.GetAsync("/"); // this returns cookies in response
            response.EnsureSuccessStatusCode();
           var responseString = await response.Content.ReadAsStringAsync();
            Assert.Contains("A new list item", responseString);

        }
    }
}
