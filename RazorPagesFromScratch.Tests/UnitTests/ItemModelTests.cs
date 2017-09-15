using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.EntityFrameworkCore;
using RazorPagesFromScratch.Models;
using Xunit;

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
            var firstItem = new Item();
            firstItem.Text = "The first (ever) list item";
            db.Items.Add(firstItem);
            db.SaveChanges();

            var secondItem = new Item();
            secondItem.Text = "Item the second";
            db.Items.Add(secondItem);
            db.SaveChanges();

            var savedItems = db.Items.ToList();
            Assert.Equal(2, savedItems.Count);

            var firstSavedItem = savedItems[0];
            var secondSavedItem = savedItems[1];
            Assert.Equal("The first (ever) list item", firstSavedItem.Text);
            Assert.Equal("Item the second", secondSavedItem.Text);
        }
    }
}
