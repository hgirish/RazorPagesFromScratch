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
            var response = await PostTextAsync(correctList, itemText, $"/lists/{correctList.Id}/");
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
            var response = await PostTextAsync(correctList, itemText, $"/lists/{correctList.Id}/");
            response.AssertRedircts($"/Lists/{correctList.Id}");
        }

     

        
    }
}
