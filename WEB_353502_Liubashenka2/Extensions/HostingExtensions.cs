namespace WEB_353502_Liubashenka2.Extensions
{
    public static class HostingExtensions
    {
        public static void RegisterCustomServices(
            this Microsoft.AspNetCore.Builder.WebApplicationBuilder builder)
        {
            builder.Services.AddScoped<ICategoryService, MemoryCategoryService>();
            builder.Services.AddScoped<IProductService, MemoryProductService>();
        }
    }
}