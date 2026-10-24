using personapi_dotnet.Models.DTOs;

namespace personapi_dotnet.Services.Interfaces;

public interface IEstudioService
{
    Task<IReadOnlyList<EstudioDto>> GetAllAsync();

    Task<EstudioDto> GetByIdAsync(int idProf, int ccPer);

    Task<EstudioDto> CreateAsync(EstudioDto dto);

    Task<EstudioDto> UpdateAsync(int idProf, int ccPer, EstudioDto dto);

    Task DeleteAsync(int idProf, int ccPer);
}
