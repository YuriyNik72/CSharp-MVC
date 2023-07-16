using Microsoft.Extensions.FileProviders;
using Npgsql;
using WebApp.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddOpenApiDocument();
builder.Services.AddTransient<NpgsqlConnection>(s => new NpgsqlConnection(builder.Configuration.GetConnectionString("Files")));
builder.Services.AddScoped<FileService>();
builder.Services.AddControllers();  // добавляем поддержку контроллеров
var app = builder.Build();
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(
        Path.Combine(builder.Environment.ContentRootPath, "View")),
    RequestPath = "/views"
});
app.UseOpenApi();
app.UseSwaggerUi3();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();