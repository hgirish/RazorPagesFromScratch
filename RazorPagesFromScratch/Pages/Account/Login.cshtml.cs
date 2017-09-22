using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorPagesFromScratch.Models;
using Microsoft.AspNetCore.Identity;

namespace RazorPagesFromScratch.Pages.Account
{
    public class LoginModel : PageModel
    {
        readonly AppDbContext dbContext;
        private readonly SignInManager<ListUser> signInManager;

        public LoginModel(AppDbContext dbContext, SignInManager<ListUser> signInManager)
        {
            this.dbContext = dbContext;
            this.signInManager = signInManager;
        }
       // [BindProperty]
       // public string Uid { get; set; }
        public async Task<IActionResult> OnGetAsync(string uid)
        {
            if (!string.IsNullOrEmpty(uid))
            {
                var user = Authenticate(uid);
                if (user != null)
                {
                    Console.Error.WriteLine("got user from authenticate");
                    await  signInManager.SignInAsync(user, true);
                }
                else
                {
                    Console.Error.WriteLine("No user from authenticate");
                }
            }
            else
            {
                Console.Error.WriteLine("No uid on login");
            }
            return Redirect("/");
        }
        private ListUser Authenticate(string uid)
        {
            Console.Error.WriteLine("Inside authenticate");
            var token = dbContext.Tokens.Where(x => x.Uid == uid).FirstOrDefault();
            if (token == null)
            {
                Console.Error.WriteLine("No token found");
                return null;
            }
            Console.Error.WriteLine("Got token");
            ListUser user = GetUser(token.Email);
            if (user == null)
            {
                Console.Error.WriteLine("ListUser does not exist");
                user = new ListUser {
                    Email = token.Email 
                ,UserName = token.Email
              // ,EmailConfirmed = true
                  //   ,PasswordHash = "password"
                      ,SecurityStamp = Guid.NewGuid().ToString()
                };
                dbContext.ListUsers.Add(user);
                dbContext.SaveChanges();
            }
            else
            {
                Console.Error.WriteLine("got user");
            }
            return user;

        }

        private ListUser GetUser(string email)
        {
            return dbContext.ListUsers.Where(x => x.Email == email).FirstOrDefault();
        }
    }
}