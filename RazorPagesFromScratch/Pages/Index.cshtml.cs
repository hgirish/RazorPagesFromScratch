
using System.Net.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace RazorPagesFromScratch.Pages
{
    public class IndexModel : PageModel
    {
        [BindProperty]
        public string item_text { get; set; }
        public IActionResult OnGet()
        {
            System.Console.WriteLine("Request Method: {0}", Request.Method);
            return Page();
        }
        public IActionResult OnPost()
        {
            System.Console.WriteLine("Request Method: {0}",Request.Method);
            if (!ModelState.IsValid)
            {
                return Page();
            }
            return Page();
        }
       
    }
}