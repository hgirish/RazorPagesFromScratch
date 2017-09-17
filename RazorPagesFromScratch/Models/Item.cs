namespace RazorPagesFromScratch.Models
{
    public class Item
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public TodoList List { get; set; }
    }
}
