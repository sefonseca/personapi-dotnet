using Microsoft.EntityFrameworkCore;
using personapi_dotnet.Data;
using personapi_dotnet.Repositories.Interfaces;

namespace personapi_dotnet.Repositories.Implementations;

/// <summary>
/// Implementación base del DAO sobre Entity Framework Core.
/// Centraliza el acceso al DbContext para no repetir código en cada repositorio.
/// </summary>
public abstract class RepositoryBase<TEntity> : IRepository<TEntity> where TEntity : class
{
    protected readonly PersonaDbContext Context;
    protected readonly DbSet<TEntity> DbSet;

    protected RepositoryBase(PersonaDbContext context)
    {
        Context = context;
        DbSet = context.Set<TEntity>();
    }

    public virtual async Task<IReadOnlyList<TEntity>> GetAllAsync()
        => await DbSet.AsNoTracking().ToListAsync();

    public virtual async Task AddAsync(TEntity entity)
    {
        await DbSet.AddAsync(entity);
        await Context.SaveChangesAsync();
    }

    public virtual async Task UpdateAsync(TEntity entity)
    {
        // Si la entidad ya viene rastreada por el contexto (se consultó para editarla),
        // EF detecta los cambios solo; Update() se usa únicamente para entidades desconectadas.
        if (Context.Entry(entity).State == EntityState.Detached)
            DbSet.Update(entity);

        await Context.SaveChangesAsync();
    }

    public virtual async Task DeleteAsync(TEntity entity)
    {
        DbSet.Remove(entity);
        await Context.SaveChangesAsync();
    }
}
