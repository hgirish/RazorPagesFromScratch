using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace RazorPagesFromScratch.Tests.UnitTests
{
    public class NonExistingPageShould :  IClassFixture<TestFixture<Startup>>
    {
        private readonly HttpClient _client;
        public NonExistingPageShould(TestFixture<Startup> fixture)
        {
            _client = fixture.Client;
        }
        [Fact]
        public async Task Returns404()
        {
            var response = await _client.GetAsync("/404-no-existent-page");

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }
    }
}
