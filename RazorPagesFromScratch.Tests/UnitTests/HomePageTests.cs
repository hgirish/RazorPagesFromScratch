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
            /*
            var getResponse = await _client.GetAsync("/");
            getResponse.EnsureSuccessStatusCode();
            string antiForgeryToken = await AntiForgeryHelper.ExtractAntiForgeryToken(getResponse);

            var formContent = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string,string>("item_text","A new list item"),
                new KeyValuePair<string, string>("__RequestVerificationToken", antiForgeryToken)

            });
            System.Console.WriteLine("__RequestVerificationToken: {0}", antiForgeryToken);
     
            var response = await _client.PostAsync("/Index", formContent);
            response.EnsureSuccessStatusCode();
            string responseString = await response.Content.ReadAsStringAsync();
            System.Console.WriteLine("responseString: {0}",responseString);
            //Assert.Contains("A new list item", responseString);
            //Assert.Contains("<h1>To-Do lists</h1>", responseString);
            //Assert.Contains("<table id=\"id_list_table\">", responseString);
            */
            var response = await _client.GetAsync("/"); // this returns cookies in response
            response.EnsureSuccessStatusCode();
            string antiForgeryToken = await AntiForgeryHelper.ExtractAntiForgeryToken(response);
            var formPostBodyData = new Dictionary<string, string>
      {
        {"__RequestVerificationToken", antiForgeryToken}, // Add token
        {"Awesomesauce.Foo", "Bar"},
        {"Awesomesauce.AnotherKey", "Baz"},
        {"item_text", "A new list item"}
      };
            // Copy cookies from response
            var requestMessage = PostRequestHelper.CreateWithCookiesFromResponse("/", formPostBodyData, response);
             response = await _client.SendAsync(requestMessage);
            response.EnsureSuccessStatusCode();
            string responseString = await response.Content.ReadAsStringAsync();
            System.Console.WriteLine("responseString: {0}", responseString);
            Assert.Contains("A new list item", responseString);
            Assert.Contains("<h1>Your To-Do list</h1>", responseString);
            Assert.Contains("<table id=\"id_list_table\">", responseString);

        }
    }
}
