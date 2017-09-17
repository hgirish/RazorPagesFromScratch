using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorPagesFromScratch.Models;

namespace RazorPagesFromScratch.Pages.Lists
{
    public class IndexModel : PageModel
    {
        private readonly AppDbContext _context;
        public IndexModel(AppDbContext context)
        {
            _context = context;
        }
        [BindProperty]
        public Item Item { get; set; }
        public List<Item> Items { get; set; } = new List<Item>();
        public IActionResult OnGet(int id)
        {
            var items = from m in _context.Items where m.List.Id == id select m;
            Items = items.ToList();

            return Page();
        }
    }
}