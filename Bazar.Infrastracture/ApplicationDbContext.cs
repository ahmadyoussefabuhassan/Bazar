using Bazar.Domain;
using Bazar.Domain.Entites;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Bazar.Infrastracture
{
    public class ApplicationDbContext : IdentityDbContext<User, IdentityRole<int>, int>
    {

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {/*
          */
        }
        override protected void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // Product -> Category: One product belongs to one category, one category has many product
            modelBuilder.Entity<Product>()
                .HasOne(p => p.Category)
                .WithMany(c => c.Products)
                .HasForeignKey(p => p.CategoryId)
                .OnDelete(DeleteBehavior.Cascade);
            //----------------------------------------------------------------------------------------------
            // Product -> Advertisements: One product belongs to one advertisement, one advertisement has many products
            modelBuilder.Entity<Product>()
                .HasOne(p => p.Advertisements)
                .WithMany(a => a.Products)
                .HasForeignKey(p => p.AdvertisementsId)
                .OnDelete(DeleteBehavior.NoAction);
            //----------------------------------------------------------------------------------------------
            // Product -> Images: One product can have many images, each image belongs to one product
            modelBuilder.Entity<Product>()
                .HasMany(p => p.Images)
                .WithOne(i => i.Product)
                .HasForeignKey(i => i.ProductId)
                .OnDelete(DeleteBehavior.NoAction);

            //----------------------------------------------------------------------------------------------
            // Advertisements -> Category: One advertisement belongs to one category, one category has many advertisements
            modelBuilder.Entity<Advertisements>()
                .HasOne(a => a.Category)
                .WithMany(c => c.Advertisements)
                .HasForeignKey(a => a.CategoryId)
                .OnDelete(DeleteBehavior.NoAction);
            //----------------------------------------------------------------------------------------------
            // Advertisements -> User: One advertisement belongs to one user, one user has many advertisements
            modelBuilder.Entity<Advertisements>()
                .HasOne(a => a.User)
                .WithMany(u => u.Advertisements)
                .HasForeignKey(a => a.UserId)
                .OnDelete(DeleteBehavior.NoAction);
            //----------------------------------------------------------------------------------------------
    
        }
       
        public DbSet<Advertisements> Advertisements { get; set; }
        public  DbSet<Product> Products { get; set; }
        public  DbSet<Category> Categories { get; set; }
        public DbSet<Images> Images { get; set; }
    }
}
