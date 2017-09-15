
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
        public List<Item> Items { get; set; }
        public IActionResult OnGet()
        {
            var items = from m in _context.Items select m;
            Items = items.ToList();
            System.Console.WriteLine("items counts:{0}", Items.Count);
            return Page();
        }
        public async Task<IActionResult> OnPost()
        {
            
            if (!ModelState.IsValid)
            {
                return Page();
            }
            _context.Items.Add(Item);
            await _context.SaveChangesAsync();
            return RedirectToPage("./Index");
        }
       
    }
}