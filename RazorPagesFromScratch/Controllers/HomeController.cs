using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RazorPagesFromScratch.Models;

namespace RazorPagesFromScratch.Controllers
{
    public class HomeController : Controller
    {
        readonly AppDbContext _dbContext;

        public HomeController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        

        public IActionResult ValidateText(Item item)
        {
            
            if (CheckTextExists(item.Text, item.ListId))
            {
                return Json(data: "You've already got this in your list");
            }

            return Json(data: true);
            //return Json(!CheckTextExists(text, listId) ?
            //"true" : string.Format("The text  {0} already exists for list {1}.", text, listId));
        }

    

        private bool CheckTextExists(string text, int listId)
        {
           var items = _dbContext.Items.Where(x => x.Text == text && x.ListId == listId);
            
            return items.Count() > 0;
            
        }
    }
}
