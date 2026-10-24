namespace personapi_dotnet.Repositories.Interfaces;

/// <summary>
/// Contrato DAO genérico con las operaciones CRUD comunes a todas las entidades.
/// Las búsquedas por llave están en cada interfaz específica, porque cada tabla
/// tiene un tipo de llave distinto (int, string o compuesta).
/// </summary>
public interface IRepository<TEntity> where TEntity : class
{
    Task<IReadOnlyList<TEntity>> GetAllAsync();

    Task AddAsync(TEntity entity);

    Task UpdateAsync(TEntity entity);

    Task DeleteAsync(TEntity entity);
}
