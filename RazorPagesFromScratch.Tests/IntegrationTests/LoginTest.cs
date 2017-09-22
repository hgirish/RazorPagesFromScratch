using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using FluentAssertions;
using OpenQA.Selenium;
using Xunit;

namespace RazorPagesFromScratch.Tests.IntegrationTests
{
    [Trait("Category","Auth")]
  public  class LoginTest : SeleniumTestFixture<IntegrationTestStartup>
    {
        string TEST_EMAIL = "edith@example.com";
        string SUBJECT = "Your login link for Superlists";
        [Fact]
        public void test_can_get_email_link_to_log_in()
        {
            // Edith goes to the awesome superlists site
            // and notices a "Log in" section in the navbar for the first time
            // It's telling her to enter her email address, so she does
            webDriver.Url = BaseAddress;
            var inputBox = GetEmailInputBox();
            inputBox.SendKeys(TEST_EMAIL);
            inputBox.SendKeys(Keys.Enter);
            string errorText = string.Empty;
            WaitFor(() =>
            {
                errorText = webDriver.FindElement(By.TagName("body")).Text;
                return !string.IsNullOrEmpty(errorText);
            });
            errorText.Should().Contain("Check your email");

            var emailFile = @"c:\temp\mailbox\email.eml";
            string text = System.IO.File.ReadAllText(emailFile);
            text.Should().Contain(SUBJECT);
            text.Should().Contain("Use this link to log in");
            Regex regex = new Regex(@"http://.+/.+$");
            var match = regex.Match(text);
            match.Success.Should().BeTrue();
            var url = match.Groups[0].Value;
            url.Should().StartWith(BaseAddress);

            webDriver.Url = url;
            WaitFor(() =>
            {
                var linkButton = webDriver.FindElement(By.CssSelector(".navbar-btn"));
                Console.WriteLine($"linkButtton.Text {linkButton.Text}");
               //var link = linkButton.FindElement(By.LinkText("Log out"));
                
                return linkButton.Text.Contains("Log out");
            });
            var navbar = webDriver.FindElement(By.CssSelector(".navbar"));
            navbar.Text.Should().Contain(TEST_EMAIL);
        }
    }
}
