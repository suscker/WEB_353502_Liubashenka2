using WEB_353502_Liubashenka2.Models;
using WEB_353502_Liubashenka2.Services.CategoryService;
using WEB_353502_Liubashenka2.Services.ProductService;

namespace WEB_353502_Liubashenka2.Extensions
{
    public static class HostingExtensions
    {
        public static void RegisterCustomServices(
            this Microsoft.AspNetCore.Builder.WebApplicationBuilder builder)
        {
            // Получение UriData из конфигурации
            var uriData = builder.Configuration.GetSection("UriData").Get<UriData>();
            if (uriData == null || string.IsNullOrEmpty(uriData.ApiUri))
            {
                throw new InvalidOperationException("UriData:ApiUri не настроен в appsettings.json");
            }

            // Регистрация HttpClient для API сервисов
            builder.Services.AddHttpClient<IProductService, ApiProductService>(opt => 
                opt.BaseAddress = new Uri(uriData.ApiUri + "dish/"));
            
            builder.Services.AddHttpClient<ICategoryService, ApiCategoryService>(opt => 
                opt.BaseAddress = new Uri(uriData.ApiUri + "categories/"));
        }
    }
}