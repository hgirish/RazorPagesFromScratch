using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace RazorPagesFromScratch.Tests.UnitTests
{
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
    }
}
