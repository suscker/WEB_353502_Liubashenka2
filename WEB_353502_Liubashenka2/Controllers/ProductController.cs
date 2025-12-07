using Microsoft.AspNetCore.Mvc;

namespace WEB_353502_Liubashenka2.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;

        public ProductController(IProductService productService, ICategoryService categoryService)
        {
            _productService = productService;
            _categoryService = categoryService;
        }

        public async Task<IActionResult> Index(string? category, int pageNo = 1)
        {
            // Получаем категории для фильтра
            var categoryResponse = await _categoryService.GetCategoryListAsync();
            var categories = categoryResponse.Successful1 ? categoryResponse.Data : new List<Category>();

            ViewData["Categories"] = categories;

            // Текущая категория
            string currentCategoryName = "Все";
            if (!string.IsNullOrEmpty(category))
            {
                var currentCategory = categories?.FirstOrDefault(c => c.NormalizedName == category);
                currentCategoryName = currentCategory?.Name ?? "Все";
            }
            ViewData["CurrentCategory"] = currentCategoryName;
            ViewData["CurrentCategoryNormalized"] = category;
            ViewData["CurrentPage"] = pageNo;

            // Получаем блюда с фильтрацией и пагинацией
            var productResponse = await _productService.GetProductListAsync(category, pageNo);

            if (!productResponse.Successful1)
            {
                ViewData["ErrorMessage"] = productResponse.ErrorMessage;
                return View("Error");
            }

            return View(productResponse.Data); // Теперь передаем ListModel<Dish>
        }
    }
}