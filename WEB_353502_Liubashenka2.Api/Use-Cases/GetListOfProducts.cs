using MediatR;
using Microsoft.EntityFrameworkCore;
using WEB_353502_Liubashenka2.Api.Data;
using WEB_353502_Liubashenka2.Domain.Entities;
using WEB_353502_Liubashenka2.Domain.Models;

namespace WEB_353502_Liubashenka2.Api.Use_Cases
{
    public sealed record GetListOfProducts(
        string? categoryNormalizedName,
        int pageNo = 1,
        int pageSize = 3) : IRequest<ResponseData<ListModel<Dish>>>;

    public class GetListOfProductsHandler : IRequestHandler<GetListOfProducts, ResponseData<ListModel<Dish>>>
    {
        private readonly AppDbContext _db;
        private readonly int _maxPageSize = 20;

        public GetListOfProductsHandler(AppDbContext db)
        {
            _db = db;
        }

        public async Task<ResponseData<ListModel<Dish>>> Handle(GetListOfProducts request, CancellationToken cancellationToken)
        {
            try
            {
                // Ограничение размера страницы
                var pageSize = Math.Min(request.pageSize, _maxPageSize);
                if (pageSize < 1) pageSize = 3;

                // Фильтрация по категории
                var query = _db.Dishes.Include(d => d.Category).AsQueryable();

                if (!string.IsNullOrEmpty(request.categoryNormalizedName))
                {
                    query = query.Where(d => d.Category != null && 
                                           d.Category.NormalizedName == request.categoryNormalizedName);
                }

                // Подсчет общего количества
                var totalItems = await query.CountAsync(cancellationToken);
                var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

                // Корректировка номера страницы
                var pageNo = request.pageNo;
                if (pageNo < 1) pageNo = 1;
                if (pageNo > totalPages && totalPages > 0) pageNo = totalPages;

                // Получение данных с пагинацией
                var dishes = await query
                    .OrderBy(d => d.Id)
                    .Skip((pageNo - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync(cancellationToken);

                var listModel = new ListModel<Dish>
                {
                    Items = dishes,
                    CurrentPage = pageNo,
                    TotalPages = totalPages > 0 ? totalPages : 1
                };

                return ResponseData<ListModel<Dish>>.Success(listModel);
            }
            catch (Exception ex)
            {
                return ResponseData<ListModel<Dish>>.Error($"Ошибка при получении списка блюд: {ex.Message}");
            }
        }
    }
}

