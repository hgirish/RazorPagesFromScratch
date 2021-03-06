
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorPagesFromScratch.Models;
using System.Linq;
namespace RazorPagesFromScratch.Pages
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
        public IActionResult OnGet()
        {
            var items = from m in _context.Items select m;
            Items = items?.ToList();
            
            return Page();
        }
      
       
    }
}