using System;
using System.Diagnostics;
using System.Threading;
using FluentAssertions;
using OpenQA.Selenium;
using Xunit;

namespace RazorPagesFromScratch.Tests.IntegrationTests
{
    [Trait("Category", "JS")]
    public class ListItemValidationTest
       : SeleniumTestFixture<IntegrationTestStartup>
    {

        [Fact]        
        public void test_cannot_add_empty_list_items()
        {
            // Edith goes to the home page and accidentally tries to submit
            // an empty list item. She hits Enter on the empty input box
            webDriver.Url = BaseAddress;
            var inputBox = FindItemTextInputBox();
            inputBox.SendKeys(Keys.Enter);

            // The home page refreshes, and there is an error message saying
            // that list items cannot be blank
            string errorText= string.Empty;
            WaitFor(() =>
            {
                errorText = webDriver.FindElement(By.CssSelector(".field-validation-error")).Text;
                return !string.IsNullOrEmpty(errorText);

            });
            //Console.WriteLine(errorText);
           errorText.Should().Be("You can't have an empty list item");

            // She tries again with some text for the item, which now works
            FindItemTextInputBox().SendKeys("Buy milk");
            FindItemTextInputBox().SendKeys(Keys.Enter);
            WaitForRowInListTable("1: Buy milk");
            
            // Perversely, she now decides to submit a second blank list item
            //FindItemTextInputBox().SendKeys("Buy milk");
            FindItemTextInputBox().SendKeys(Keys.Enter);

            // She receives a similar warning on the list page
            WaitFor(() =>
            {
                errorText = webDriver.FindElement(By.CssSelector(".field-validation-error")).Text;
                return !string.IsNullOrEmpty(errorText);

            });
            errorText.Should().Be("You can't have an empty list item");


            // And she can correct it by filling some text in
            FindItemTextInputBox().SendKeys("Make tea");
            FindItemTextInputBox().SendKeys(Keys.Enter);
            WaitForRowInListTable("1: Buy milk");
            WaitForRowInListTable("2: Make tea");
           /* */
            
        }
        [Fact]
        public void test_cannot_add_duplicate_items()
        {
            // Edith goes to the home page and starts a new list
            webDriver.Url = BaseAddress;
            const string buyWellies = "Buy wellies";
            FindItemTextInputBox().SendKeys(buyWellies);
            FindItemTextInputBox().SendKeys(Keys.Enter);
            WaitForRowInListTable("1: Buy wellies");

            // She accidentally tries to enter a duplicate item
            FindItemTextInputBox().SendKeys(buyWellies);
            FindItemTextInputBox().SendKeys(Keys.Enter);

            // She sees a helpful error message
            var errorText = string.Empty;
            WaitFor(() =>
            {
                errorText = webDriver.FindElement(By.CssSelector(".field-validation-error")).Text;
                return !string.IsNullOrEmpty(errorText);
            });
            errorText.Should().Be("You've already got this in your list");
        }

      
    }
}
