using System.Linq;
using System.Threading;
using OpenQA.Selenium;
using Xunit;

namespace RazorPagesFromScratch.Tests.IntegrationTests
{
    [Trait("Category", "Integration")]
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
            // Edith has heard about a cool new online to-do app. She goes
            // to check out its homepage
            webDriver.Url = _baseAddress;
            // She notices the page title and header mention to-do lists
            var body = webDriver.FindElement(By.TagName("body")).Text;
            Assert.NotEmpty(body);
            Assert.Contains("Your To-Do list", body);
            Assert.Equal("To-Do lists", webDriver.Title);

            //She is invited to enter a to -do item straight away
            var inputbox = webDriver.FindElement(By.Id("id_new_item"));
            Assert.Equal("Enter a to-do item", inputbox.GetAttribute("placeholder"));

            //She types "Buy peacock feathers" into a text box (Edith's hobby
            // is tying fly-fishing lures)
            inputbox.SendKeys("Buy peacock feathers");

            // When she hits enter, the page updates, now the page lists
            // "1: Buy peacock feathers" as an item in a to-do list table
            inputbox.SendKeys(Keys.Enter);
            Thread.Sleep(1000);

            var table = webDriver.FindElement(By.Id("id_list_table"));
            var rows = table.FindElements(By.TagName("tr"));
            Assert.Contains(rows, row => row.Text == "1: Buy peacock feathers");

        }
    }
}
