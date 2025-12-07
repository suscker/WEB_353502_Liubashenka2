using WEB_353502_Liubashenka2.Domain.Entities;

namespace WEB_353502_Liubashenka2.Api.Data
{
    public static class DbInitializer
    {
        public static void SeedData(AppDbContext context, IConfiguration configuration)
        {
            var baseUrl = configuration["ApiSettings:BaseUrl"];

            Console.WriteLine("Creating database...");
            context.Database.EnsureCreated();
            Console.WriteLine("Database created. Seeding data...");

            // Очистка
            context.Dishes.RemoveRange(context.Dishes);
            context.Categories.RemoveRange(context.Categories);
            context.SaveChanges();

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
            context.SaveChanges();

            // Блюда
            var dishes = new List<Dish>
            {
                new() { Name="Суп харчо", Description="Очень острый, невкусный", Price = 5m,
                    Image=$"{baseUrl}/images/soup.jpg", CategoryId=categories[0].Id },

                new() { Name="Борщ", Description="Много сала, без сметаны", Price = 6m,
                    Image=$"{baseUrl}/images/borscht.jpg", CategoryId=categories[0].Id },

                new() { Name="Цезарь", Description="С курицей и пармезаном", Price = 8m,
                    Image=$"{baseUrl}/images/caesar.jpg", CategoryId=categories[1].Id },

                new() { Name="Шашлык", Description="Из свинины", Price = 12m,
                    Image=$"{baseUrl}/images/meat.jpg", CategoryId=categories[2].Id },

                new() { Name="Чай", Description="Черный", Price = 2m,
                    Image=$"{baseUrl}/images/tea.jpg", CategoryId=categories[3].Id },

                new() { Name="Тирамису", Description="Итальянский десерт", Price = 5m,
                    Image=$"{baseUrl}/images/dessert.jpg", CategoryId=categories[4].Id }
            };

            context.Dishes.AddRange(dishes);
            context.SaveChanges();

            Console.WriteLine("Database seeded!");
        }
    }
}
