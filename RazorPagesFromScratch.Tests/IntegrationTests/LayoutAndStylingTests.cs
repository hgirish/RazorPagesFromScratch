using FluentAssertions;
using OpenQA.Selenium;
using System;
using Xunit;

namespace RazorPagesFromScratch.Tests.IntegrationTests
{
    public class LayoutAndStylingTests : SeleniumTestFixture<IntegrationTestStartup>
    {
        [Fact]
        [Trait("Category", "UI")]
        public void test_layout_and_styling()
        {
            // Edith goes to the home page
            webDriver.Url = BaseAddress;
            webDriver.Manage().Window.Size = new System.Drawing.Size(1024, 768);

            // She notices the input box is nicely centered
            var inputbox = FindItemTextInputBox();
            AssertCentered(inputbox);

            // she starts a new list and sees the input is nicely centered there too
            inputbox.SendKeys("testing");
            inputbox.SendKeys(Keys.Enter);
            WaitForRowInListTable("1: testing");
            inputbox = FindItemTextInputBox();
            AssertCentered(inputbox);

        }
        private static void AssertCentered(IWebElement inputbox)
        {
            var x = inputbox.Location.X;
            var y = inputbox.Location.Y;
            var width = inputbox.Size.Width;

            var horizontalPosition = x + (width / 2);
            var delta = Math.Abs(horizontalPosition - 512);
            Console.WriteLine($"X: {x}, Y: {y}, width: {width}, horizontal postion:{horizontalPosition}, delta: {delta}");
            delta.Should().BeLessOrEqualTo(10);
        }

    }
}
