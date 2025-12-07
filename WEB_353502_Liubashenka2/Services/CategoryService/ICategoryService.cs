namespace WEB_353502_Liubashenka2.Services.CategoryService
{
    public interface ICategoryService
    {
        public Task<ResponseData<List<Category>>> GetCategoryListAsync();
    }
}