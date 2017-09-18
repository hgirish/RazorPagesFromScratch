using System.Linq;
using System.Threading;
using FluentAssertions;
using OpenQA.Selenium;
using Xunit;
using System;
using System.Diagnostics;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Chrome;

namespace RazorPagesFromScratch.Tests.IntegrationTests
{
    [Trait("Category", "Integration")]
    public class NewVisitorTest : SeleniumTestFixture<IntegrationTestStartup>
    {
        private readonly string _baseAddress;
        private IWebDriver _browser;
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
        public void MultipleUsersCanStartListsAtDifferentUrls()
        {
            // Edith starts a new to do list
            _browser.Url = _baseAddress;
            var inputBox = FindItemTextInputBox();
            inputBox.SendKeys("Buy peacock feathers");
            inputBox.SendKeys(Keys.Enter);
            WaitForRowInListTable("1: Buy peacock feathers");

            // she notices that her list has a unique URL
            var edithListUrl = _browser.Url;
            Console.WriteLine($"edithListUrl: {edithListUrl}");
            edithListUrl.Should().MatchRegex(_baseAddress + "/lists/.+");

            // Now a new user, Francis, comes along to the site.
            //// We use a new browser session to make sure that no information
            //// of Edith's is coming through from cookies etc
            Console.WriteLine("Quit the browser");
            _browser.Quit();
            Console.WriteLine("Start new browser");
            _browser = new ChromeDriver();
            // Francis visits the home page. There is no sign of Edith's
            // list
            Console.WriteLine($"Francis goes to url: {_baseAddress} ");
            _browser.Navigate().GoToUrl(_baseAddress);
            
            var page_text = _browser.FindElement(By.TagName("body")).Text;
            Console.WriteLine($"francis page text: {page_text}");
            page_text.Should().NotContain("Buy peacock feathers");
            page_text.Should().NotContain("make a fly");
            IWebElement inputbox = FindItemTextInputBox();
            inputbox.SendKeys("Buy milk");
            inputbox.SendKeys(Keys.Enter);
            WaitForRowInListTable("1: Buy milk");
            // Francis gets his own unique URL
            var francis_list_url = _browser.Url;
            Console.WriteLine($"francis_list_url: {francis_list_url}");
            francis_list_url.Should().MatchRegex(_baseAddress + "/lists/.+");
            francis_list_url.Should().NotBeSameAs(edithListUrl);


            // Again, there is no trace of Edith"s list
            page_text = _browser.FindElement(By.TagName("body")).Text;
            page_text.Should().NotContain("Buy peacock feathers");
            page_text.Should().Contain("Buy milk");

            // Satisfied, they both go back to sleep
        }
        [Fact]
        public void TestLayoutAndStyling()
        {
            // Edith goes to the home page
            _browser.Url = _baseAddress;
            _browser.Manage().Window.Size = new System.Drawing.Size(1024, 768);

            // She notices the input box is nicely centered
            var inputbox = FindItemTextInputBox();
            var x = inputbox.Location.X;
            var y = inputbox.Location.Y;
            var width = inputbox.Size.Width;
           
            var horizontalPosition = x + (width / 2);
            var delta = Math.Abs(horizontalPosition - 512);
            Console.WriteLine($"X: {x}, Y: {y}, width: {width}, horizontal postion:{horizontalPosition}, delta: {delta}");
            delta.Should().BeLessOrEqualTo(10);
        }

        private IWebElement FindItemTextInputBox()
        {

            // Francis starts a new list by entering a new item. He
            // is less interesting than Edith...
            return _browser.FindElement(By.Name("Item.Text"));
        }

        private void CheckForRowInListTable(string rowText)
        {
            var table = _browser.FindElement(By.Id("id_list_table"));
            var rows = table.FindElements(By.TagName("tr"));
            //Assert.Contains(rows, row => row.Text == rowText);
            rows.Should().Contain(row => row.Text == rowText, $"[{rowText}] not found in table");
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
                    Console.WriteLine("Elapsed miliseconds: {0}", sw.ElapsedMilliseconds);
                    sw.Stop();
                    return;
                }
                catch (WebDriverException)
                {
                    if (sw.ElapsedMilliseconds > MAX_WAIT)
                    {
                        Console.WriteLine("Elapsed miliseconds: {0}", sw.ElapsedMilliseconds);
                        sw.Stop();
                        _browser.Quit();
                        throw;
                    }
                    Console.WriteLine("Going to sleep for half second");
                    Thread.Sleep(500);

                }
            }

        }
    }
}
