using Holtz.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Holtz.CQRS.Infraestructure.Persistence
{
    public class ApplicationContext : DbContext
    {
        public ApplicationContext() { }
        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options) { }

        public DbSet<Product> Products { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite(@"DataSource=data.db;");
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>(entity =>
            {
                entity.ToTable("Products");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).IsRequired();
                entity.Property(e => e.Price).IsRequired();
                entity.Property(e => e.Description).HasMaxLength(255).IsRequired();

                // Mock data
                //entity.HasData(new Product("MockDb", "Description", 15.50));
            });
            base.OnModelCreating(modelBuilder);
        }
    }
}
