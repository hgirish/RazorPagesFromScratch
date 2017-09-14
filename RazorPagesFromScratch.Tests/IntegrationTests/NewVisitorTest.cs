using OpenQA.Selenium;
using Xunit;

namespace RazorPagesFromScratch.Tests.IntegrationTests
{
    public class NewVisitorTest : IClassFixture<SeleniumTestFixture<Startup>>
    {
        private readonly string _baseAddress;
        private readonly IWebDriver webDriver;
        public NewVisitorTest(SeleniumTestFixture<Startup> fixture)
        {
            _baseAddress = fixture.BaseAddress;
            webDriver = fixture.webDriver;
        }

        [Fact]
        public void CanStartAListAndRetrieveItLater()
        {
            webDriver.Url = _baseAddress;
            var body = webDriver.FindElement(By.TagName("body")).Text;
            Assert.NotEmpty(body);
            Assert.Equal("To-Do", body);
            Assert.Equal("To-Do", webDriver.Title);
        }
    }
}
