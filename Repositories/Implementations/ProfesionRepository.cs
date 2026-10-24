using Microsoft.EntityFrameworkCore;
using personapi_dotnet.Data;
using personapi_dotnet.Models.Entities;
using personapi_dotnet.Repositories.Interfaces;

namespace personapi_dotnet.Repositories.Implementations;

public class ProfesionRepository : RepositoryBase<Profesion>, IProfesionRepository
{
    public ProfesionRepository(PersonaDbContext context) : base(context) { }

    public override async Task<IReadOnlyList<Profesion>> GetAllAsync()
        => await DbSet.AsNoTracking()
            .OrderBy(p => p.Id)
            .ToListAsync();

    public Task<Profesion?> GetByIdAsync(int id)
        => DbSet.FirstOrDefaultAsync(p => p.Id == id);

    public Task<bool> ExistsAsync(int id)
        => DbSet.AnyAsync(p => p.Id == id);

    public Task<bool> HasRelatedRecordsAsync(int id)
        => Context.Set<Estudio>().AnyAsync(e => e.IdProf == id);
}
