using personapi_dotnet.Models.Entities;

namespace personapi_dotnet.Repositories.Interfaces;

public interface IPersonaRepository : IRepository<Persona>
{
    Task<Persona?> GetByIdAsync(int cc);

    /// <summary>Persona con sus teléfonos y estudios (incluye la profesión de cada estudio).</summary>
    Task<Persona?> GetWithRelationsAsync(int cc);

    Task<bool> ExistsAsync(int cc);

    /// <summary>Indica si la persona tiene teléfonos o estudios asociados.</summary>
    Task<bool> HasRelatedRecordsAsync(int cc);
}
