using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RazorPagesFromScratch.Models
{
    public class PasswordlessAuthenticationBackend
    {
        private readonly AppDbContext dbContext;

        public PasswordlessAuthenticationBackend(AppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public ListUser Authenticate(string uid)
        {
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
                user = new ListUser { Email = token.Email };
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
