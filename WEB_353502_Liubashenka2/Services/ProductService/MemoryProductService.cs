using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace WEB_353502_Liubashenka2.Services.ProductService
{
    public class MemoryProductService : IProductService
    {
        private List<Dish> _dishes;
        private readonly IConfiguration _configuration;
        private readonly ICategoryService _categoryService;

        public MemoryProductService(ICategoryService categoryService, IConfiguration configuration)
        {
            _configuration = configuration;
            _categoryService = categoryService;
            SetupData();
        }

        /// <summary>
        /// Инициализация списков
        /// </summary>
        private void SetupData()
        {
            // Получаем категории
            var categories = _categoryService.GetCategoryListAsync().Result.Data;

            _dishes = new List<Dish>
            {
                new Dish
                {
                    Id = 1,
                    Name = "Суп харчо",
                    Description = "Очень острый, невкусный",
                    Price = 5,
                    Image = "/images/soup.jpg",
                    Category = categories?.Find(c => c.NormalizedName == "soups"),
                    CategoryId = categories?.Find(c => c.NormalizedName == "soups")?.Id
                },
                new Dish
                {
                    Id = 2,
                    Name = "Борщ",
                    Description = "Много сала, без сметаны",
                    Price = 6,
                    Image = "/images/borscht.jpg",
                    Category = categories?.Find(c => c.NormalizedName == "soups"),
                    CategoryId = categories?.Find(c => c.NormalizedName == "soups")?.Id
                },
                new Dish
                {
                    Id = 3,
                    Name = "Цезарь",
                    Description = "С курицей и пармезаном",
                    Price = 8,
                    Image = "/images/caesar.jpg",
                    Category = categories?.Find(c => c.NormalizedName == "salads"),
                    CategoryId = categories?.Find(c => c.NormalizedName == "salads")?.Id
                },
                new Dish
                {
                    Id = 4,
                    Name = "Шашлык",
                    Description = "Из свинины",
                    Price = 12,
                    Image = "/images/meat.jpg",
                    Category = categories?.Find(c => c.NormalizedName == "main-dishes"),
                    CategoryId = categories?.Find(c => c.NormalizedName == "main-dishes")?.Id
                },
                new Dish
                {
                    Id = 5,
                    Name = "Чай",
                    Description = "Черный",
                    Price = 2,
                    Image = "/images/tea.jpg",
                    Category = categories?.Find(c => c.NormalizedName == "drinks"),
                    CategoryId = categories?.Find(c => c.NormalizedName == "drinks")?.Id
                },
                new Dish
                {
                    Id = 6,
                    Name = "Тирамису",
                    Description = "Итальянский десерт",
                    Price = 5,
                    Image = "/images/dessert.jpg",
                    Category = categories?.Find(c => c.NormalizedName == "desserts"),
                    CategoryId = categories?.Find(c => c.NormalizedName == "desserts")?.Id
                }
            };
        }

        public Task<ResponseData<ListModel<Dish>>> GetProductListAsync(string? categoryNormalizedName, int pageNo = 1)
        {
            // Фильтрация по категории
            var filteredDishes = _dishes
                .Where(d => categoryNormalizedName == null ||
                           d.Category?.NormalizedName == categoryNormalizedName)
                .ToList();

            // Пагинация
            int pageSize = _configuration.GetValue<int>("ItemsPerPage", 3);
            int totalItems = filteredDishes.Count;
            int totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

            // Корректировка номера страницы
            if (pageNo < 1) pageNo = 1;
            if (pageNo > totalPages && totalPages > 0) pageNo = totalPages;

            var pagedDishes = filteredDishes
                .Skip((pageNo - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            var listModel = new ListModel<Dish>
            {
                Items = pagedDishes,
                CurrentPage = pageNo,
                TotalPages = totalPages > 0 ? totalPages : 1
            };

            return Task.FromResult(ResponseData<ListModel<Dish>>.Success(listModel));
        }

        // Остальные методы пока не реализуем - заглушки
        public Task<ResponseData<Dish>> GetProductByIdAsync(int id)
        {
            return Task.FromResult(ResponseData<Dish>.Error("Метод не реализован"));
        }

        public Task<ResponseData<Dish>> UpdateProductAsync(int id, Dish product, IFormFile? formFile)
        {
            return Task.FromResult(ResponseData<Dish>.Error("Метод не реализован"));
        }

        public Task<ResponseData<bool>> DeleteProductAsync(int id)
        {
            return Task.FromResult(ResponseData<bool>.Error("Метод не реализован", false));
        }

        public Task<ResponseData<Dish>> CreateProductAsync(Dish product, IFormFile? formFile)
        {
            return Task.FromResult(ResponseData<Dish>.Error("Метод не реализован"));
        }
    }
}