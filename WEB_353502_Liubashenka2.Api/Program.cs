using Microsoft.EntityFrameworkCore;
using WEB_353502_Liubashenka2.Api.Data;
using WEB_353502_Liubashenka2.Api.Use_Cases;
using MediatR;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("Default")));

builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseStaticFiles();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

var dishGroup = app.MapGroup("/api/dish").WithTags("Dish");
var categoryGroup = app.MapGroup("/api/categories").WithTags("Categories");

dishGroup.MapGet("/{category:alpha?}",
    async (IMediator mediator, string? category, int pageNo = 1, int pageSize = 3) =>
    {
        var data = await mediator.Send(new GetListOfProducts(category, pageNo, pageSize));
        return TypedResults.Ok(data);
    })
    .WithName("GetAllDishes");

categoryGroup.MapGet("/",
    async (IMediator mediator) =>
    {
        var data = await mediator.Send(new GetListOfCategories());
        return TypedResults.Ok(data);
    })
    .WithName("GetAllCategories");

using (var scope = app.Services.CreateScope())
{
    try
    {
        var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        var config = scope.ServiceProvider.GetRequiredService<IConfiguration>();

        await DbInitializer.SeedData(context, config);
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error seeding database: {ex.Message}");
    }
}

app.Run();
