using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace RazorPagesFromScratch.Tests.UnitTests
{
    public class WebDefaultRequestShould : IClassFixture<TestFixture<Startup>>
    {
        //private readonly TestServer _server;
        private readonly HttpClient _client;
        public WebDefaultRequestShould(TestFixture<Startup> fixture)
        {
            //_server = new TestServer(new WebHostBuilder()
            //    .UseContentRoot(@"D:\Dev\Projects\Learn\AspNetCore\RazorPagesFromScratch\RazorPagesFromScratch")
            //    .UseStartup<Startup>());
            //_client = _server.CreateClient();
            _client = fixture.Client;
        }
        [Fact]
        public async Task ReturnsHelloWorld()
        {
            // Act 
            var response = await _client.GetAsync("/");
            response.EnsureSuccessStatusCode();

            var responseString = await response.Content.ReadAsStringAsync();

            // Assert
            Assert.Contains("To-Do", responseString);



        }
        
    }
}
