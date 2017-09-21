using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using RazorPagesFromScratch.Models;
using Xunit;
using System.Linq;
using FluentAssertions;

namespace RazorPagesFromScratch.Tests.UnitTests
{
    [Trait("Category", "Unit")]
    [Trait("Name","ListView")]
    public class ListViewTest : TestBase
    {
        [Fact]
        public async Task test_can_save_a_POST_request_to_an_existing_listAsync()
        {
            var text = "A new item for an existing list";
            TodoList correctList = SeedTodoList();
           var response = await PostTextAsync(correctList, text, $"/lists/{correctList.Id}/");
            db.Items.Count().Should().Be(1);
            var new_item = db.Items.First();
            new_item.Text.Should().Be(text);
            new_item.List.Should().Equals(correctList);
        }
        [Fact]
        public async Task test_POST_redirects_to_list_viewAsync()
        {
            var text = "A new item for an existing list";
            TodoList correctList = SeedTodoList();
            var response = await PostTextAsync(correctList, text, $"/lists/{correctList.Id}");
            response.Headers.Location.Should().Be($"/Lists/{correctList.Id}");
        }
    }
}
