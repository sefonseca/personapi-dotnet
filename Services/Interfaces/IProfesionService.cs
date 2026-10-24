using personapi_dotnet.Models.DTOs;

namespace personapi_dotnet.Services.Interfaces;

public interface IProfesionService
{
    Task<IReadOnlyList<ProfesionDto>> GetAllAsync();

    Task<ProfesionDto> GetByIdAsync(int id);

    Task<ProfesionDto> CreateAsync(ProfesionDto dto);

    Task<ProfesionDto> UpdateAsync(int id, ProfesionDto dto);

    Task DeleteAsync(int id);
}
