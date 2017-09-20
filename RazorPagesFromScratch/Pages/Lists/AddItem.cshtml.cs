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
    public class AddItemModel : PageModel
    {
        private readonly AppDbContext _context;

        public AddItemModel(AppDbContext context)
        {
            _context = context;
        }
        [BindProperty]
        public Item Item { get; set; }
        public List<Item> Items { get; set; }
        public IActionResult OnGet(int id)
        {
            var items = from m in _context.Items where m.List.Id == id select m;
            Items = items?.ToList();

            return Page();
        }
        public IActionResult OnPost(int id)
        {

            if (!ModelState.IsValid)
            {
                List<KeyValuePair<string, string>> erors = new List<KeyValuePair<string, string>>();
                foreach (var kvp in ViewData.ModelState)
                {
                    if (kvp.Value.Errors.Count() > 0)
                    {
                        var key = kvp.Key;

                        var message = kvp.Value.Errors.First().ErrorMessage;
                        KeyValuePair<string, string> dict = new KeyValuePair<string, string>(key, message);

                        Console.WriteLine($"{key}: {message}");
                        erors.Add(dict);
                    }
                }
                var jsonErrors = JsonConvert.SerializeObject(erors);
                TempData["ModelState"] = jsonErrors;
                var url = $"/lists/{id}";
                ViewData["listId"] = id;

                return RedirectToPage("Index",routeValues:new { id = id });
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