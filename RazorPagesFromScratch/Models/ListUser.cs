using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RazorPagesFromScratch.Models
{
    public class ListUser : IdentityUser
    {
       // public int Id { get; set; }
       // public string Email { get; set; }
        public bool IsAdmin()
        {
            return Email == "admin@example.com";
        }
        public bool IsActive()
        {
            return true;
        }
    }
}
