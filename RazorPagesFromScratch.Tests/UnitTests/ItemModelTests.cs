using System.Linq;
using Microsoft.EntityFrameworkCore;
using RazorPagesFromScratch.Models;
using Xunit;
using FluentAssertions;

namespace RazorPagesFromScratch.Tests.UnitTests
{
    [Trait("Category", "Model")]
    public class ItemModelTests
    {
        [Fact]
        public void SavingAndRetrievingItems()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "Superlists")
                .Options;

            var db = new AppDbContext(options);

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
    }
}
