using personapi_dotnet.Models.DTOs;

namespace personapi_dotnet.Services.Interfaces;

public interface IPersonaService
{
    Task<IReadOnlyList<PersonaDto>> GetAllAsync();

    Task<PersonaDto> GetByIdAsync(int cc);

    Task<PersonaDetalleDto> GetDetalleAsync(int cc);

    Task<PersonaDto> CreateAsync(PersonaDto dto);

    Task<PersonaDto> UpdateAsync(int cc, PersonaDto dto);

    Task DeleteAsync(int cc);
}
