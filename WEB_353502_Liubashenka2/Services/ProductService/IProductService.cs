using Microsoft.AspNetCore.Http;

namespace WEB_353502_Liubashenka2.Services.ProductService
{
    public interface IProductService
    {
        /// <summary>
        /// Получение списка всех объектов
        /// </summary>
        /// <param name="categoryNormalizedName">нормализованное имя категории для фильтрации</param>
        /// <param name="pageNo">номер страницы списка</param>
        /// <returns></returns>
        public Task<ResponseData<ListModel<Dish>>> GetProductListAsync(string? categoryNormalizedName, int pageNo = 1);

        /// <summary>
        /// Поиск объекта по Id
        /// </summary>
        /// <param name="id">Идентификатор объекта</param>
        /// <returns>Найденный объект или null, если объект не найден</returns>
        public Task<ResponseData<Dish>> GetProductByIdAsync(int id);

        /// <summary>
        /// Обновление объекта
        /// </summary>
        /// <param name="id">Id изменяемого объекта</param>
        /// <param name="product">объект с новыми параметрами</param>
        /// <param name="formFile">Файл изображения</param>
        /// <returns></returns>
        public Task<ResponseData<Dish>> UpdateProductAsync(int id, Dish product, IFormFile? formFile);

        /// <summary>
        /// Удаление объекта
        /// </summary>
        /// <param name="id">Id удаляемого объекта</param>
        /// <returns></returns>
        public Task<ResponseData<bool>> DeleteProductAsync(int id);

        /// <summary>
        /// Создание объекта
        /// </summary>
        /// <param name="product">Новый объект</param>
        /// <param name="formFile">Файл изображения</param>
        /// <returns>Созданный объект</returns>
        public Task<ResponseData<Dish>> CreateProductAsync(Dish product, IFormFile? formFile);
    }
}