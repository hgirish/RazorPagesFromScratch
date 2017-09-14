using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using Xunit;

namespace RazorPagesFromScratch.Tests.IntegrationTests
{
   public class SeleniumFixtureTests : IClassFixture<SeleniumTestFixture<Startup>>
    {
        private string _baseAddress;
        public SeleniumFixtureTests(SeleniumTestFixture<Startup> fixture)
        {
            _baseAddress = fixture.BaseAddress;
        }

        [Fact]
        public async Task ReturnsHelloWorld()
        {
            using (IWebDriver webDriver = new FirefoxDriver())
            {
                webDriver.Url = _baseAddress;
                var body = webDriver.FindElement(By.TagName("body")).Text;
                Assert.NotEmpty(body);
                Assert.Equal("Hello World!", body);
                
            }
           



        }
    }
}
