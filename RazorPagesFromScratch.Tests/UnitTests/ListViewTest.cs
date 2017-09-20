using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using RazorPagesFromScratch.Models;
using Xunit;

namespace RazorPagesFromScratch.Tests.UnitTests
{
    public class ListViewTest : TestBase
    {
        [Fact]
        public async Task test_can_save_a_POST_request_to_an_existing_listAsync()
        {
            var text = "A new item for an existing list";
            TodoList correctList = SeedTodoList();
            PostTextAsync(correctList,text, $"/lists/")
        }
    }
}
