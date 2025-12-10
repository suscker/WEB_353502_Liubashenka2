using MediatR;
using Microsoft.EntityFrameworkCore;
using WEB_353502_Liubashenka2.Api.Data;
using WEB_353502_Liubashenka2.Domain.Entities;
using WEB_353502_Liubashenka2.Domain.Models;

namespace WEB_353502_Liubashenka2.Api.Use_Cases
{
    public sealed record GetListOfCategories() : IRequest<ResponseData<List<Category>>>;

    public class GetListOfCategoriesHandler : IRequestHandler<GetListOfCategories, ResponseData<List<Category>>>
    {
        private readonly AppDbContext _db;

        public GetListOfCategoriesHandler(AppDbContext db)
        {
            _db = db;
        }

        public async Task<ResponseData<List<Category>>> Handle(GetListOfCategories request, CancellationToken cancellationToken)
        {
            try
            {
                var categories = await _db.Categories
                    .OrderBy(c => c.Id)
                    .ToListAsync(cancellationToken);

                return ResponseData<List<Category>>.Success(categories);
            }
            catch (Exception ex)
            {
                return ResponseData<List<Category>>.Error($"Ошибка при получении списка категорий: {ex.Message}");
            }
        }
    }
}

