
using System.Net.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace RazorPagesFromScratch.Pages
{
    public class IndexModel : PageModel
    {
        public IActionResult OnGet()
        {
            
            return Page();
        }
       
    }
}