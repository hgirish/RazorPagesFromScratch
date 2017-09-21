using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorPagesFromScratch.Models;
using Newtonsoft.Json;

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
            if (TempData.ContainsKey("ModelState"))
            {
                var tempdata = TempData["ModelState"].ToString();
                var errors = JsonConvert.DeserializeObject<List<KeyValuePair<string,string>>>(tempdata);
                if (errors != null)
                {
                    foreach (KeyValuePair<string, string> item in errors)
                    {
                        Console.WriteLine($"{item.Key}:{item.Value}");
                        ModelState.AddModelError(item.Key, item.Value);
                    }
                }

                //ModelState.AddModelError("Item.Text", "You can't have an empty list item");

            }

            return Page();
        }
        public IActionResult OnPost(int id)
        {

            if (!ModelState.IsValid)
            {
                return Page();
            }
            var existingItem = _context.Items.Where(x => x.Text == Item.Text && x.ListId == id);
            if (existingItem.Any())
            {
                ModelState.AddModelError("Item.Text", "You've already got this in your list");
                return Page();
            }

            var list = _context.TodoLists.Find(id);
            Item.List = list;
            _context.Items.Add(Item);
            _context.SaveChanges();
            return RedirectToPage("Index", routeValues: new { id = id });
            //return Redirect($"/lists/{list.Id}/");
        }
    }
}