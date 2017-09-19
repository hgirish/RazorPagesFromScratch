using FluentAssertions;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using Xunit;

namespace RazorPagesFromScratch.Tests.IntegrationTests
{
    public class SimpleListCreationTest : SeleniumTestFixture<IntegrationTestStartup>
    {
        
        [Fact]
        public void test_can_start_a_list_for_one_user()
        {
            
            
            // Edith has heard about a cool new online to-do app. She goes
            // to check out its homepage
            webDriver.Url = BaseAddress;
            // She notices the page title and header mention to-do lists
            var body = webDriver.FindElement(By.TagName("body")).Text;
            Assert.NotEmpty(body);
            Assert.Contains("Your To-Do list", body);
            Assert.Equal("To-Do lists", webDriver.Title);

            //She is invited to enter a to -do item straight away
            var inputbox = FindItemTextInputBox();
            Assert.Equal("Enter a to-do item", inputbox.GetAttribute("placeholder"));

            //She types "Buy peacock feathers" into a text box (Edith's hobby
            // is tying fly-fishing lures)
            inputbox.SendKeys("Buy peacock feathers");

            // When she hits enter, the page updates, now the page lists
            // "1: Buy peacock feathers" as an item in a to-do list table
            inputbox.SendKeys(Keys.Enter);
            //Thread.Sleep(1000);


            WaitForRowInListTable("1: Buy peacock feathers");

            //There is still a text box inviting her to add another item. She
            // enters "Use peacock feathers to make a fly" (Edith is very
            // methodical)
            inputbox = FindItemTextInputBox();
            inputbox.SendKeys("Use peacock feathers to make a fly");
            inputbox.SendKeys(Keys.Enter);
            //Thread.Sleep(1000);

            // The page updates again, and now shows both items on her list              
            WaitForRowInListTable("2: Use peacock feathers to make a fly");
            WaitForRowInListTable("1: Buy peacock feathers");

            // Satisfied she goes back to sleep.


        }
        [Fact]
        public void test_multiple_users_can_start_lists_at_different_urls()
        {
            // Edith starts a new to do list
            webDriver.Url = BaseAddress;
            var inputBox = FindItemTextInputBox();
            inputBox.SendKeys("Buy peacock feathers");
            inputBox.SendKeys(Keys.Enter);
            WaitForRowInListTable("1: Buy peacock feathers");

            // she notices that her list has a unique URL
            var edithListUrl = webDriver.Url;
            Console.WriteLine($"edithListUrl: {edithListUrl}");
            edithListUrl.Should().MatchRegex(BaseAddress + "/lists/.+");

            // Now a new user, Francis, comes along to the site.
            //// We use a new browser session to make sure that no information
            //// of Edith's is coming through from cookies etc
            Console.WriteLine("Quit the browser");
            webDriver.Quit();
            Console.WriteLine("Start new browser");
            webDriver = new ChromeDriver();
            // Francis visits the home page. There is no sign of Edith's
            // list
            Console.WriteLine($"Francis goes to url: {BaseAddress} ");
            webDriver.Navigate().GoToUrl(BaseAddress);

            var page_text = webDriver.FindElement(By.TagName("body")).Text;
            Console.WriteLine($"francis page text: {page_text}");
            page_text.Should().NotContain("Buy peacock feathers");
            page_text.Should().NotContain("make a fly");
            IWebElement inputbox = FindItemTextInputBox();
            inputbox.SendKeys("Buy milk");
            inputbox.SendKeys(Keys.Enter);
            WaitForRowInListTable("1: Buy milk");
            // Francis gets his own unique URL
            var francis_list_url = webDriver.Url;
            Console.WriteLine($"francis_list_url: {francis_list_url}");
            francis_list_url.Should().MatchRegex(BaseAddress + "/lists/.+");
            francis_list_url.Should().NotBeSameAs(edithListUrl);


            // Again, there is no trace of Edith"s list
            page_text = webDriver.FindElement(By.TagName("body")).Text;
            page_text.Should().NotContain("Buy peacock feathers");
            page_text.Should().Contain("Buy milk");

            // Satisfied, they both go back to sleep
        }



        protected void CheckForRowInListTable(string rowText)
        {
            var table = webDriver.FindElement(By.Id("id_list_table"));
            var rows = table.FindElements(By.TagName("tr"));
            //Assert.Contains(rows, row => row.Text == rowText);
            rows.Should().Contain(row => row.Text == rowText, $"[{rowText}] not found in table");
        }
    }
}
