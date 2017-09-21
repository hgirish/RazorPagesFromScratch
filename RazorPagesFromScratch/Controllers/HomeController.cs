using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RazorPagesFromScratch.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult ValidateText(string text, int listId)
        {
            return Json(CheckTextExists(text, listId) ?
            "true" : string.Format("The text  {0} already exists for list {1}.", text, listId));
        }

        private bool CheckTextExists(string text, int listId)
        {
            return false;
        }
    }
}
