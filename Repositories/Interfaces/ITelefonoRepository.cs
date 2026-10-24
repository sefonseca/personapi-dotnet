using personapi_dotnet.Models.Entities;

namespace personapi_dotnet.Repositories.Interfaces;

public interface ITelefonoRepository : IRepository<Telefono>
{
    /// <summary>Teléfono con los datos de su dueño.</summary>
    Task<Telefono?> GetByIdAsync(string num);

    Task<bool> ExistsAsync(string num);
}
