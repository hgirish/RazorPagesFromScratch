using System.Linq;
using System.Threading;
using FluentAssertions;
using OpenQA.Selenium;
using Xunit;
using System;
using System.Diagnostics;

namespace RazorPagesFromScratch.Tests.IntegrationTests
{
    [Trait("Category", "Integration")]
    public class NewVisitorTest : SeleniumTestFixture<IntegrationTestStartup>
    {
        private readonly string _baseAddress;
        private readonly IWebDriver _browser;
        int MAX_WAIT = 10000;
        public NewVisitorTest()
        {
            _baseAddress = BaseAddress;
            _browser = webDriver;
        }

        [Fact]
        public void CanStartAListAndRetrieveItLater()
        {
            // Edith has heard about a cool new online to-do app. She goes
            // to check out its homepage
            _browser.Url = _baseAddress;
            // She notices the page title and header mention to-do lists
            var body = _browser.FindElement(By.TagName("body")).Text;
            Assert.NotEmpty(body);
            Assert.Contains("Your To-Do list", body);
            Assert.Equal("To-Do lists", _browser.Title);

            //She is invited to enter a to -do item straight away
            var inputbox = _browser.FindElement(By.Id("Item_Text"));
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
            inputbox = _browser.FindElement(By.Name("Item.Text"));
            inputbox.SendKeys("Use peacock feathers to make a fly");
            inputbox.SendKeys(Keys.Enter);
            //Thread.Sleep(1000);

            // The page updates again, and now shows both items on her list              
            WaitForRowInListTable("2: Use peacock feathers to make a fly");
            WaitForRowInListTable("1: Buy peacock feathers");

            // Satisfied she goes back to sleep.


        }
        [Fact]
        public void MultipleUsersCanStartListsAtDifferentUrls()
        {
            // Edith starts a new to do list
            _browser.Url = _baseAddress;
            var inputBox = _browser.FindElement(By.Name("Item.Text"));
            inputBox.SendKeys("Buy peacock feathers");
            inputBox.SendKeys(Keys.Enter);
            WaitForRowInListTable("1: Buy peacock feathers");

            // she notices that her list has a unique URL
            var edithListUrl = _browser.Url;
            Console.WriteLine(edithListUrl);
        }
        private void CheckForRowInListTable(string rowText)
        {
           var table = _browser.FindElement(By.Id("id_list_table"));
           var rows = table.FindElements(By.TagName("tr"));
            //Assert.Contains(rows, row => row.Text == rowText);
            rows.Should().Contain(row => row.Text == rowText,$"[{rowText}] not found in table");
        }
        private void WaitForRowInListTable(string rowText)
        {
            var startTime = DateTime.Now;
            Stopwatch sw = Stopwatch.StartNew();

            while (true)
            {
                try
                {
                    var table = _browser.FindElement(By.Id("id_list_table"));
                    var rows = table.FindElements(By.TagName("tr"));
                    //Assert.Contains(rows, row => row.Text == rowText);
                    rows.Should().Contain(row => row.Text == rowText, $"[{rowText}] not found in table");
                    Console.WriteLine("Elapsed miliseconds: {0}",sw.ElapsedMilliseconds);
                    sw.Stop();
                    return;
                }
                catch (WebDriverException)
                {
                    if (sw.ElapsedMilliseconds > MAX_WAIT)
                    {
                        sw.Stop();
                        throw;
                    }
                    Console.WriteLine("Going to sleep for half second");
                    Thread.Sleep(500);
                    
                }
            }

        }
    }
}
