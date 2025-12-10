using Microsoft.EntityFrameworkCore;
using WEB_353502_Liubashenka2.Domain.Entities;

namespace WEB_353502_Liubashenka2.Api.Data
{
    public static class DbInitializer
    {
        public static async Task SeedData(AppDbContext context, IConfiguration configuration)
        {
            var baseUrl = configuration["ApiSettings:BaseUrl"];

            Console.WriteLine("Applying migrations...");
            await context.Database.MigrateAsync();
            Console.WriteLine("Migrations applied. Seeding data...");

            // Полная очистка перед повторным заполнением
            context.Dishes.RemoveRange(context.Dishes);
            context.Categories.RemoveRange(context.Categories);
            await context.SaveChangesAsync();

            // Категории
            var categories = new List<Category>
            {
                new() { Name = "Супы", NormalizedName = "soups" },
                new() { Name = "Салаты", NormalizedName = "salads" },
                new() { Name = "Основные блюда", NormalizedName = "main-dishes" },
                new() { Name = "Напитки", NormalizedName = "drinks" },
                new() { Name = "Десерты", NormalizedName = "desserts" }
            };

            context.Categories.AddRange(categories);
            await context.SaveChangesAsync();

            // Блюда
            var dishes = new List<Dish>
            {
                new() { Name="Суп харчо", Description="Очень острый, невкусный", Price = 5m,
                    Image=$"{baseUrl}/Images/soup.jpg", CategoryId=categories[0].Id },

                new() { Name="Борщ", Description="Много сала, без сметаны", Price = 6m,
                    Image=$"{baseUrl}/Images/borscht.jpg", CategoryId=categories[0].Id },

                new() { Name="Цезарь", Description="С курицей и пармезаном", Price = 8m,
                    Image=$"{baseUrl}/Images/caesar.jpg", CategoryId=categories[1].Id },

                new() { Name="Шашлык", Description="Из свинины", Price = 12m,
                    Image=$"{baseUrl}/Images/meat.jpg", CategoryId=categories[2].Id },

                new() { Name="Чай", Description="Черный", Price = 2m,
                    Image=$"{baseUrl}/Images/tea.jpg", CategoryId=categories[3].Id },

                new() { Name="Тирамису", Description="Итальянский десерт", Price = 5m,
                    Image=$"{baseUrl}/Images/dessert.jpg", CategoryId=categories[4].Id }
            };

            context.Dishes.AddRange(dishes);
            await context.SaveChangesAsync();

            Console.WriteLine("Database seeded!");
        }
    }
}
