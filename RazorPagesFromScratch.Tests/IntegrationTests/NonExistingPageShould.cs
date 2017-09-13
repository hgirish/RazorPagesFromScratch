using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace RazorPagesFromScratch.Tests.IntegrationTests
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
