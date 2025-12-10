using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.OpenApi;
using WEB_353502_Liubashenka2.Api.Data;
using WEB_353502_Liubashenka2.Domain.Entities;
namespace WEB_353502_Liubashenka2.Api.EndPoints;

public static class DishEndpoints
{
    public static void MapDishEndpoints (this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/Dish").WithTags(nameof(Dish));

        group.MapGet("/", async (AppDbContext db) =>
        {
            return await db.Dishes.ToListAsync();
        })
        .WithName("GetAllDishes")
        .WithOpenApi();

        group.MapGet("/{id}", async Task<Results<Ok<Dish>, NotFound>> (int id, AppDbContext db) =>
        {
            return await db.Dishes.AsNoTracking()
                .FirstOrDefaultAsync(model => model.Id == id)
                is Dish model
                    ? TypedResults.Ok(model)
                    : TypedResults.NotFound();
        })
        .WithName("GetDishById")
        .WithOpenApi();

        group.MapPut("/{id}", async Task<Results<Ok, NotFound>> (int id, Dish dish, AppDbContext db) =>
        {
            var affected = await db.Dishes
                .Where(model => model.Id == id)
                .ExecuteUpdateAsync(setters => setters
                    .SetProperty(m => m.Id, dish.Id)
                    .SetProperty(m => m.Name, dish.Name)
                    .SetProperty(m => m.Description, dish.Description)
                    .SetProperty(m => m.Price, dish.Price)
                    .SetProperty(m => m.Image, dish.Image)
                    .SetProperty(m => m.MimeType, dish.MimeType)
                    .SetProperty(m => m.CategoryId, dish.CategoryId)
                    );
            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("UpdateDish")
        .WithOpenApi();

        group.MapPost("/", async (Dish dish, AppDbContext db) =>
        {
            db.Dishes.Add(dish);
            await db.SaveChangesAsync();
            return TypedResults.Created($"/api/Dish/{dish.Id}",dish);
        })
        .WithName("CreateDish")
        .WithOpenApi();

        group.MapDelete("/{id}", async Task<Results<Ok, NotFound>> (int id, AppDbContext db) =>
        {
            var affected = await db.Dishes
                .Where(model => model.Id == id)
                .ExecuteDeleteAsync();
            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("DeleteDish")
        .WithOpenApi();
    }
}
