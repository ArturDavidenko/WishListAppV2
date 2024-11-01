using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using WebAPIWishList.Models;

namespace WebAPIWishList.Data
{
    public class DBContext : IdentityDbContext<IdentityUser>
    {
        public DbSet<WishItem> wishItems { get; set; }
        public DBContext(DbContextOptions<DBContext> options) : base(options) { }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<WishItem>()
                .HasKey(w => w.Id);
            modelBuilder.Entity<WishItem>()
                .Property(w => w.Id)
                .ValueGeneratedOnAdd(); // Указывает, что Id будет генерироваться при добавлении



            modelBuilder.Entity<WishItem>()
                .HasOne(w => w.User)
                .WithMany()
                .HasForeignKey(w => w.UserId);
        }
    }
}
