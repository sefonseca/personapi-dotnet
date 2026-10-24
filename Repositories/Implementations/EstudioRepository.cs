using Microsoft.EntityFrameworkCore;
using personapi_dotnet.Data;
using personapi_dotnet.Models.Entities;
using personapi_dotnet.Repositories.Interfaces;

namespace personapi_dotnet.Repositories.Implementations;

public class EstudioRepository : RepositoryBase<Estudio>, IEstudioRepository
{
    public EstudioRepository(PersonaDbContext context) : base(context) { }

    public override async Task<IReadOnlyList<Estudio>> GetAllAsync()
        => await DbSet.AsNoTracking()
            .Include(e => e.IdProfNavigation)
            .Include(e => e.CcPerNavigation)
            .OrderBy(e => e.CcPer)
            .ThenBy(e => e.IdProf)
            .ToListAsync();

    public Task<Estudio?> GetByIdAsync(int idProf, int ccPer)
        => DbSet.Include(e => e.IdProfNavigation)
            .Include(e => e.CcPerNavigation)
            .FirstOrDefaultAsync(e => e.IdProf == idProf && e.CcPer == ccPer);

    public Task<bool> ExistsAsync(int idProf, int ccPer)
        => DbSet.AnyAsync(e => e.IdProf == idProf && e.CcPer == ccPer);
}
