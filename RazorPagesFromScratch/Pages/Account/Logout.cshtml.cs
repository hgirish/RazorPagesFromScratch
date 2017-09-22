using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Identity;
using RazorPagesFromScratch.Models;

namespace RazorPagesFromScratch.Pages.Account
{
    public class LogoutModel : PageModel
    {
        private readonly SignInManager<ListUser> _signInManager;
        public LogoutModel(SignInManager<ListUser> signInManager)
        {
            _signInManager = signInManager;
        }

        public async Task<IActionResult> OnPostAsync()
        {
          await  _signInManager.SignOutAsync();
            return Redirect("/");
        }
    }
}