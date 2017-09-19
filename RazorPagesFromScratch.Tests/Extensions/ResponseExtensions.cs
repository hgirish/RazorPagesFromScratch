using FluentAssertions;
using System.Net;
using System.Net.Http;

namespace RazorPagesFromScratch.Tests.Extensions
{
    public static  class ResponseExtensions
    {
        public static void AssertRedircts(this HttpResponseMessage response, string redirectLocation)
        {
            response.StatusCode.Should().Be(HttpStatusCode.Redirect);
            response.Headers.Location.ToString().Should().Be(redirectLocation);
        }
    }
}
