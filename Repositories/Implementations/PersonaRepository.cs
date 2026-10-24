using Microsoft.EntityFrameworkCore;
using personapi_dotnet.Data;
using personapi_dotnet.Models.Entities;
using personapi_dotnet.Repositories.Interfaces;

namespace personapi_dotnet.Repositories.Implementations;

public class PersonaRepository : RepositoryBase<Persona>, IPersonaRepository
{
    public PersonaRepository(PersonaDbContext context) : base(context) { }

    public override async Task<IReadOnlyList<Persona>> GetAllAsync()
        => await DbSet.AsNoTracking()
            .OrderBy(p => p.Apellido)
            .ThenBy(p => p.Nombre)
            .ToListAsync();

    public Task<Persona?> GetByIdAsync(int cc)
        => DbSet.FirstOrDefaultAsync(p => p.Cc == cc);

    public Task<Persona?> GetWithRelationsAsync(int cc)
        => DbSet.AsNoTracking()
            .Include(p => p.Telefonos)
            .Include(p => p.Estudios).ThenInclude(e => e.IdProfNavigation)
            .FirstOrDefaultAsync(p => p.Cc == cc);

    public Task<bool> ExistsAsync(int cc)
        => DbSet.AnyAsync(p => p.Cc == cc);

    public async Task<bool> HasRelatedRecordsAsync(int cc)
        => await Context.Set<Telefono>().AnyAsync(t => t.Duenio == cc)
           || await Context.Set<Estudio>().AnyAsync(e => e.CcPer == cc);
}
