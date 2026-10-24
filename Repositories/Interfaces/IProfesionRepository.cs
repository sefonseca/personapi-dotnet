using personapi_dotnet.Models.Entities;

namespace personapi_dotnet.Repositories.Interfaces;

public interface IProfesionRepository : IRepository<Profesion>
{
    Task<Profesion?> GetByIdAsync(int id);

    Task<bool> ExistsAsync(int id);

    /// <summary>Indica si hay estudios registrados con esta profesión.</summary>
    Task<bool> HasRelatedRecordsAsync(int id);
}
