using System.Linq;
using System.Threading;
using FluentAssertions;
using OpenQA.Selenium;
using Xunit;

namespace RazorPagesFromScratch.Tests.IntegrationTests
{
    [Trait("Category", "Integration")]
    public class NewVisitorTest : IClassFixture<SeleniumTestFixture<IntegrationTestStartup>>
    {
        private readonly string _baseAddress;
        private readonly IWebDriver webDriver;
        public NewVisitorTest(SeleniumTestFixture<IntegrationTestStartup> fixture)
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
            var inputbox = webDriver.FindElement(By.Id("Item_Text"));
            Assert.Equal("Enter a to-do item", inputbox.GetAttribute("placeholder"));

            //She types "Buy peacock feathers" into a text box (Edith's hobby
            // is tying fly-fishing lures)
            inputbox.SendKeys("Buy peacock feathers");

            // When she hits enter, the page updates, now the page lists
            // "1: Buy peacock feathers" as an item in a to-do list table
            inputbox.SendKeys(Keys.Enter);
            Thread.Sleep(1000);            
            
            CheckForRowInListTable("1: Buy peacock feathers");

            //There is still a text box inviting her to add another item. She
            // enters "Use peacock feathers to make a fly" (Edith is very
            // methodical)
            inputbox = webDriver.FindElement(By.Name("Item.Text"));
            inputbox.SendKeys("Use peacock feathers to make a fly");
            inputbox.SendKeys(Keys.Enter);
            Thread.Sleep(1000);

            // The page updates again, and now shows both items on her list          
            CheckForRowInListTable("2: Use peacock feathers to make a fly");




        }

        private void CheckForRowInListTable(string rowText)
        {
           var table = webDriver.FindElement(By.Id("id_list_table"));
           var rows = table.FindElements(By.TagName("tr"));
            //Assert.Contains(rows, row => row.Text == rowText);
            rows.Should().Contain(row => row.Text == rowText,$"[{rowText}] not found in table");
        }
    }
}
