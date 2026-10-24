using personapi_dotnet.Extensions;
using personapi_dotnet.Middleware;

var builder = WebApplication.CreateBuilder(args);

// ---------- Servicios (inyección de dependencias por capas) ----------
builder.Services.AddControllersWithViews();
builder.Services.AddPersistence(builder.Configuration);   // Datos: DbContext
builder.Services.AddRepositories();                        // DAO
builder.Services.AddApplicationServices();                 // Servicios / negocio
builder.Services.AddSwaggerDocumentation();                // OpenAPI 3

var app = builder.Build();

// ---------- Pipeline HTTP ----------
if (!app.Environment.IsDevelopment())
{
    // Producción: página de error amigable para las vistas MVC
    app.UseExceptionHandler("/Home/Error");
}

// Errores de la API REST -> JSON ProblemDetails (va después de UseExceptionHandler
// para atender primero las rutas /api)
app.UseMiddleware<ApiExceptionMiddleware>();

// Swagger se habilita por ambiente (ver appsettings.{Ambiente}.json -> "Swagger:Enabled")
if (app.Configuration.GetValue<bool>("Swagger:Enabled"))
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "PersonAPI v1");
        options.DocumentTitle = "PersonAPI - Swagger";
    });
}

app.UseRouting();
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllers(); // API REST (rutas por atributo /api/...)

app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets(); // Vistas MVC

app.Run();
