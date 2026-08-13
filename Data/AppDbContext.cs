using InventoryApi.Modules.Products.Entities;
using InventoryApi.Modules.Categories.Entities;
using Microsoft.EntityFrameworkCore;

namespace InventoryApi.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Product>()
                .HasOne(p => p.Category)          
                .WithMany(c => c.Products)        
                .HasForeignKey(p => p.CategoryId) 
                .OnDelete(DeleteBehavior.Restrict); 
        }

    }

}



