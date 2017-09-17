using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorPagesFromScratch.Models;

namespace RazorPagesFromScratch.Pages.Lists
{
    public class NewModel : PageModel
    {
        private readonly AppDbContext _context;
        public NewModel(AppDbContext context)
        {
            _context = context;
        }
        [BindProperty]
        public Item Item { get; set; }
        public List<Item> Items { get; set; } = new List<Item>();
        public IActionResult OnGet()
        {
            var items = from m in _context.Items select m;
            Items = items?.ToList();

            return Page();
        }
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            var list = new TodoList();
            Item.List = list;
            _context.Items.Add(Item);
            await _context.SaveChangesAsync();
            return Redirect($"/lists/{list.Id}/");
        }
    }
}