﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using RazorPagesFromScratch.Models;
using Xunit;
using System.Linq;
using FluentAssertions;
using System.Net;

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
        [Fact]
        public async Task test_validation_errors_end_up_on_lists_pageAsync()
        {
            TodoList correctList = SeedTodoList();
            var response = await PostTextAsync(correctList, "", $"/lists/{correctList.Id}/");
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            
            string expected_error =  "You can't have an empty list item";
            var responseString = await response.Content.ReadAsStringAsync();
            responseString = WebUtility.HtmlDecode(responseString);
            responseString.Should().Contain("template: list.html");
            responseString.Should().Contain(expected_error);
        }
        [Fact]
        public async Task test_duplicate_item_validation_errors_end_up_on_lists_pageAsync()
        {
            var text = "textey";
            TodoList correctList = SeedTodoList();
            var item1 = new Item { Text = text, List = correctList };
            db.Items.Add(item1);
            db.SaveChanges();
            var response = await PostTextAsync(correctList, text, $"/lists/{correctList.Id}/");
            //response.StatusCode.Should().Be(HttpStatusCode.OK);

            var responseString = await response.Content.ReadAsStringAsync();
            var expected_error = "You've already got this in your list";
            responseString = WebUtility.HtmlDecode(responseString);
            responseString.Should().Contain("template: list.html");
            responseString.Should().Contain(expected_error);
            db.Items.Count().Should().Be(1);

        }
    }
}
