using Microsoft.EntityFrameworkCore;

namespace ApiCatalog.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        public DbSet<Category>? Categories { get; set; }
        public DbSet<Product>? Products { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Category
            modelBuilder.Entity<Category>().HasKey(c => c.CategoryId);
            modelBuilder.Entity<Category>().Property(c => c.Name)
                                           .HasMaxLength(100)
                                           .IsRequired();

            modelBuilder.Entity<Category>().Property(c => c.Description)
                                           .HasMaxLength(150)
                                           .IsRequired();

            //Product
            modelBuilder.Entity<Product>().HasKey(c => c.ProductId);
            modelBuilder.Entity<Product>().Property(c => c.Name)
                                          .HasMaxLength(100)
                                          .IsRequired();

            modelBuilder.Entity<Product>().Property(c => c.Description).HasMaxLength(150);
            modelBuilder.Entity<Product>().Property(c => c.Image).HasMaxLength(100);
            modelBuilder.Entity<Product>().Property(c => c.Price).HasPrecision(14, 2);

            //Relations
            modelBuilder.Entity<Product>()
                .HasOne<Category>(c => c.Category)
                    .WithMany(p => p.Products)
                        .HasForeignKey(c => c.CategoryId);
        }
    }
}
