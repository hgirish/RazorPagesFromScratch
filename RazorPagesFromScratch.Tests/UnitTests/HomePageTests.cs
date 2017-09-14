using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace RazorPagesFromScratch.Tests.UnitTests
{
    [Trait("Category", "Unit")]
    public class HomePageTests : IClassFixture<TestFixture<Startup>>
    {
        private readonly HttpClient _client;
        public HomePageTests(TestFixture<Startup> fixture)
        {
            _client = fixture.Client;
        }
      [Fact]
      public async Task HomePageReturnsCorrectHtml()
        {
            var response = await _client.GetAsync("/");
            var html = await response.Content.ReadAsStringAsync();
            Assert.StartsWith("<html>", html);
            Assert.Contains("<title>To-Do lists</title>", html);
            Assert.EndsWith("</html>", html);

        }
        [Fact]
        public async Task HomePageCanSaveAPostRequest()
        {
            var formContent = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string,string>("item_text","A new list item")
            });
            var response = await _client.PostAsync("/", formContent);
            Assert.Contains("A new list item", await response.Content.ReadAsStringAsync());

        }
    }
}
