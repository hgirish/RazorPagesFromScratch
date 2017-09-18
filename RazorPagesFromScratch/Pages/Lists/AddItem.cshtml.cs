using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorPagesFromScratch.Models;

namespace RazorPagesFromScratch.Pages.Lists
{
    public class AddItemModel : PageModel
    {
        private readonly AppDbContext _context;

        public AddItemModel(AppDbContext context)
        {
            _context = context;
        }
        [BindProperty]
        public Item Item { get; set; }
        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync(int id)
        {
            Console.WriteLine($"AddItem OnPostAsync id: {id}");
            if (!ModelState.IsValid)
            {
                return Page();
            }
            var list = _context.TodoLists.Find(id);
            Item.List = list;
            _context.Items.Add(Item);
            await _context.SaveChangesAsync();
            return Redirect($"/lists/{list.Id}/");
        }
    }
}