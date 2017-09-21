using System.Linq;
using Microsoft.EntityFrameworkCore;
using RazorPagesFromScratch.Models;
using Xunit;
using FluentAssertions;
using System;
using System.ComponentModel.DataAnnotations;

namespace RazorPagesFromScratch.Tests.UnitTests
{
    [Trait("Category", "Model")]
    public class ItemModelTests : IDisposable
    {
        AppDbContext db;
        public ItemModelTests()
        {
            var name = System.IO.Path.GetRandomFileName();
            Console.WriteLine("dbname: {0}", name);
           var options = new DbContextOptionsBuilder<AppDbContext>()
               .UseInMemoryDatabase(databaseName: name)
               .Options;

            db = new AppDbContext(options);
        }
        [Fact]
        public void SavingAndRetrievingItems()
        {

           
            var todoList = new TodoList();
            db.TodoLists.Add(todoList);
            db.SaveChanges();

            var firstItem = new Item();
            firstItem.Text = "The first (ever) list item";
            firstItem.List = todoList;
            db.Items.Add(firstItem);
            db.SaveChanges();

            var secondItem = new Item();
            secondItem.Text = "Item the second";
            secondItem.List = todoList;
            db.Items.Add(secondItem);
            db.SaveChanges();

            var savedList = db.TodoLists.First();
            var savedItems = db.Items.ToList();
            Assert.Equal(2, savedItems.Count);

            var firstSavedItem = savedItems[0];
            var secondSavedItem = savedItems[1];

            firstSavedItem.Text.Should().Equals("The first (ever) list item");
            firstSavedItem.List.Should().Equals(todoList);

            secondSavedItem.Text.Should().Equals("Item the second");
            secondSavedItem.List.Should().Equals(todoList);

            
        }
        [Fact]
        public void test_cannot_save_empty_list_items()
        {
            var todoList = new TodoList();
            db.TodoLists.Add(todoList);
            db.SaveChanges();

            var firstItem = new Item();
            firstItem.Text = "";
            firstItem.List = todoList;
            db.Items.Add(firstItem);


            Assert.Throws<ValidationException>(() => db.SaveChanges());

        }
        [Fact]
        public void test_duplicate_items_are_invalid()
        {
           // db.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            var todolist = new TodoList();
            db.TodoLists.Add(todolist);
            db.SaveChanges();

            var firstItem = new Item();
            firstItem.Text = "bla";
            firstItem.List = todolist;
            db.Items.Add(firstItem);
            db.SaveChanges();
            var listId = db.TodoLists.First().Id;
            var secondItem = new Item();
            secondItem.Text = "bla";
            secondItem.ListId = listId;
            Assert.Throws<InvalidOperationException> (()=>db.Add(secondItem));
           // db.SaveChanges();
           // Assert.Throws<InvalidOperationException>(() => db.SaveChanges());
        }
        [Fact]
        public void test_CAN_save_same_item_to_different_lists()
        {
            var list1 = new TodoList();
            var list2 = new TodoList();
            db.TodoLists.Add(list1);
            db.TodoLists.Add(list2);
            db.SaveChanges();

            var firstItem = new Item();
            firstItem.Text = "bla";
            firstItem.List = list1;
            db.Items.Add(firstItem);
            db.SaveChanges();

            var secondItem = new Item();
            secondItem.Text = "bla";
            secondItem.List = list2;
            db.Add(secondItem);
            db.SaveChanges(); // Should not raise exception
            db.Items.Count().Should().Be(2);


        }

        public void Dispose()
        {
            db.Dispose();
        }
    }
}
