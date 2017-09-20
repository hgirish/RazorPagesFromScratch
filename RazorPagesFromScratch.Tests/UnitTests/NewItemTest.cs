using System;
using System.Collections.Generic;
using Xunit;
using FluentAssertions;
using RazorPagesFromScratch.Models;
using System.Threading.Tasks;
using System.Net.Http;
using System.Linq;
using RazorPagesFromScratch.Tests.Extensions;

namespace RazorPagesFromScratch.Tests.UnitTests
{
    [Trait("Category", "Unit")]
    [Trait("Name", "NewList")]
    public class NewItemTest : TestBase
    {
        [Fact]
        public async Task CanSaveAPostRequestToAnExistingListAsync()
        {
            TodoList correctList = SeedTodoList();
            var itemText = "A new item for an existing list";
            var response = await PostTextAsync(correctList, itemText);
            var items = db.Items;
            Console.WriteLine("items count: {0}",items.Count());
            items.Count().Should().Equals(1);
            var newItem = items.FirstOrDefault();
            newItem.Text.Should().Equals(itemText);
            newItem.List.Should().Equals(correctList);
        }
        [Fact]
        public async Task RedirectsToListViewAsync()
        {
            TodoList correctList = SeedTodoList();
            var itemText = "A new item for an existing list";
            var response = await PostTextAsync(correctList, itemText);
            response.AssertRedircts($"/Lists/{correctList.Id}");
        }

     

        private async Task<HttpResponseMessage> PostTextAsync(TodoList list,string text)
        {
            var response = await Client.GetAsync("/"); // this returns cookies in response
            response.EnsureSuccessStatusCode();
            string antiForgeryToken = await AntiForgeryHelper.ExtractAntiForgeryToken(response);
            var formPostBodyData = new Dictionary<string, string>
            {
                { "__RequestVerificationToken", antiForgeryToken}, // Add token        
                { "Item.Text", text}
            };
            // Copy cookies from response
            var requestMessage = PostRequestHelper.CreateWithCookiesFromResponse(
                $"/lists/{list.Id}/AddItem", formPostBodyData, response);
            response = await Client.SendAsync(requestMessage);
            return response;
        }
    }
}
