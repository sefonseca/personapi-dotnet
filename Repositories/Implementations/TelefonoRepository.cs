using Microsoft.EntityFrameworkCore;
using personapi_dotnet.Data;
using personapi_dotnet.Models.Entities;
using personapi_dotnet.Repositories.Interfaces;

namespace personapi_dotnet.Repositories.Implementations;

public class TelefonoRepository : RepositoryBase<Telefono>, ITelefonoRepository
{
    public TelefonoRepository(PersonaDbContext context) : base(context) { }

    public override async Task<IReadOnlyList<Telefono>> GetAllAsync()
        => await DbSet.AsNoTracking()
            .Include(t => t.DuenioNavigation)
            .OrderBy(t => t.Num)
            .ToListAsync();

    public Task<Telefono?> GetByIdAsync(string num)
        => DbSet.Include(t => t.DuenioNavigation)
            .FirstOrDefaultAsync(t => t.Num == num);

    public Task<bool> ExistsAsync(string num)
        => DbSet.AnyAsync(t => t.Num == num);
}
