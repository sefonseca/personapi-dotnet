using personapi_dotnet.Models.Entities;

namespace personapi_dotnet.Repositories.Interfaces;

public interface IEstudioRepository : IRepository<Estudio>
{
    /// <summary>Estudio (llave compuesta) con su profesión y su persona.</summary>
    Task<Estudio?> GetByIdAsync(int idProf, int ccPer);

    Task<bool> ExistsAsync(int idProf, int ccPer);
}
