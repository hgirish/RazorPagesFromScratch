using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace RazorPagesFromScratch.Models
{
    public class AppDbContext : IdentityDbContext<ListUser>
    {
      
        public AppDbContext( DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        public DbSet<Item> Items { get; set; }
        public DbSet<TodoList> TodoLists { get; set; }
        public DbSet<Token> Tokens { get; set; }
        public DbSet<ListUser> ListUsers { get; set; }

        public override int SaveChanges()
        {
            ValidateEntities();
            return base.SaveChanges();
        }
        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default(CancellationToken))
        {
            ValidateEntities();
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }
        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            ValidateEntities();
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }
        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            ValidateEntities();
            return base.SaveChangesAsync(cancellationToken);
        }

        private void ValidateEntities()
        {
            var entities = from e in ChangeTracker.Entries()
                           where e.State == EntityState.Added
                               || e.State == EntityState.Modified
                           select e.Entity;
            foreach (var entity in entities)
            {
                var validationContext = new ValidationContext(entity);
                Validator.ValidateObject(entity, validationContext);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Item>()
                .HasAlternateKey(i => new { i.ListId, i.Text })
                .HasName("AltenateKey_TextListId");
        }
        
        
    }
}
