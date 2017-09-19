using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
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
       

        public IActionResult OnPost(int id)
        {

            if (!ModelState.IsValid)
            {
                TempData["ModelState"] = ModelState;
                var url = $"/lists/Index/?id={id}";
                return Redirect(url);
            }
            var list = _context.TodoLists.Find(id);
            Item.List = list;
            _context.Items.Add(Item);
             _context.SaveChanges();
            return Redirect($"/lists/{list.Id}/");
        }
    }
}