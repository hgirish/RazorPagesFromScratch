using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RazorPagesFromScratch.Models
{
    public class ListUserManager
    {
        private readonly AppDbContext dbContext;

        public ListUserManager(AppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public void CreateUser(string email)
        {
            dbContext.ListUsers.Add(new ListUser { Email = email });

        }
    }
}
