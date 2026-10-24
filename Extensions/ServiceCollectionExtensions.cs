using System.Reflection;
using Microsoft.EntityFrameworkCore;
using personapi_dotnet.Data;
using personapi_dotnet.Repositories.Implementations;
using personapi_dotnet.Repositories.Interfaces;
using personapi_dotnet.Services.Implementations;
using personapi_dotnet.Services.Interfaces;

namespace personapi_dotnet.Extensions;

/// <summary>
/// Registro de dependencias por capa. Mantiene Program.cs limpio y deja claro
/// qué implementación se inyecta para cada interfaz.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>Capa de datos: DbContext de EF Core con la cadena del ambiente actual.</summary>
    public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");

        if (string.IsNullOrWhiteSpace(connectionString))
            throw new InvalidOperationException(
                "No se encontró 'ConnectionStrings:DefaultConnection'. " +
                "Configúrela con User Secrets (desarrollo) o con la variable de entorno " +
                "ConnectionStrings__DefaultConnection (producción). Ver README.");

        services.AddDbContext<PersonaDbContext>(options => options.UseSqlServer(connectionString));
        return services;
    }

    /// <summary>Capa DAO: repositorios.</summary>
    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IPersonaRepository, PersonaRepository>();
        services.AddScoped<IProfesionRepository, ProfesionRepository>();
        services.AddScoped<ITelefonoRepository, TelefonoRepository>();
        services.AddScoped<IEstudioRepository, EstudioRepository>();
        return services;
    }

    /// <summary>Capa de servicios: reglas de negocio.</summary>
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IPersonaService, PersonaService>();
        services.AddScoped<IProfesionService, ProfesionService>();
        services.AddScoped<ITelefonoService, TelefonoService>();
        services.AddScoped<IEstudioService, EstudioService>();
        return services;
    }

    /// <summary>Documentación OpenAPI 3 con Swagger, incluyendo los comentarios XML de los controladores.</summary>
    public static IServiceCollection AddSwaggerDocumentation(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(options =>
        {
            var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            if (File.Exists(xmlPath))
                options.IncludeXmlComments(xmlPath);
        });
        return services;
    }
}
