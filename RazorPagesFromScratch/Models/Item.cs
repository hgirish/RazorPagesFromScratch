using System.ComponentModel.DataAnnotations;

namespace RazorPagesFromScratch.Models
{
    public class Item
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "You can't have an empty list item")]
        public string Text { get; set; }
        public TodoList List { get; set; }
    }
}
