using personapi_dotnet.Models.DTOs;

namespace personapi_dotnet.Services.Interfaces;

public interface ITelefonoService
{
    Task<IReadOnlyList<TelefonoDto>> GetAllAsync();

    Task<TelefonoDto> GetByIdAsync(string num);

    Task<TelefonoDto> CreateAsync(TelefonoDto dto);

    Task<TelefonoDto> UpdateAsync(string num, TelefonoDto dto);

    Task DeleteAsync(string num);
}
