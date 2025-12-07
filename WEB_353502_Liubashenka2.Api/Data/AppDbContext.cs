using Microsoft.EntityFrameworkCore;
using WEB_353502_Liubashenka2.Domain.Entities;

namespace WEB_353502_Liubashenka2.Api.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Dish> Dishes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Настройка отношений
            modelBuilder.Entity<Dish>()
                .HasOne(d => d.Category)
                .WithMany()
                .HasForeignKey(d => d.CategoryId)
                .OnDelete(DeleteBehavior.SetNull);

            // Уникальный индекс для NormalizedName
            modelBuilder.Entity<Category>()
                .HasIndex(c => c.NormalizedName)
                .IsUnique();

            // Указываем тип для Price
            modelBuilder.Entity<Dish>()
                .Property(d => d.Price)
                .HasConversion<decimal>();
        }
    }
}