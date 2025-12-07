using Microsoft.EntityFrameworkCore;
using WEB_353502_Liubashenka2.Api.Data;
using MediatR;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// œÓ‰ÍÎ˛˜‡ÂÏ SQLite
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

// ---- »Õ»÷»¿À»«¿÷»ﬂ ¡¿«€ ----
using (var scope = app.Services.CreateScope())
{
    try
    {
        var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        var config = scope.ServiceProvider.GetRequiredService<IConfiguration>();

        DbInitializer.SeedData(context, config);
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error seeding database: {ex.Message}");
    }
}

app.Run();
