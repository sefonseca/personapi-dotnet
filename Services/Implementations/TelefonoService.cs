using personapi_dotnet.Exceptions;
using personapi_dotnet.Mappings;
using personapi_dotnet.Models.DTOs;
using personapi_dotnet.Repositories.Interfaces;
using personapi_dotnet.Services.Interfaces;

namespace personapi_dotnet.Services.Implementations;

public class TelefonoService : ITelefonoService
{
    private readonly ITelefonoRepository _repository;
    private readonly IPersonaRepository _personaRepository;

    public TelefonoService(ITelefonoRepository repository, IPersonaRepository personaRepository)
    {
        _repository = repository;
        _personaRepository = personaRepository;
    }

    public async Task<IReadOnlyList<TelefonoDto>> GetAllAsync()
        => (await _repository.GetAllAsync()).Select(t => t.ToDto()).ToList();

    public async Task<TelefonoDto> GetByIdAsync(string num)
    {
        var telefono = await _repository.GetByIdAsync(num)
            ?? throw new NotFoundException($"No existe el teléfono {num}.");
        return telefono.ToDto();
    }

    public async Task<TelefonoDto> CreateAsync(TelefonoDto dto)
    {
        var num = dto.Num.Trim();

        if (await _repository.ExistsAsync(num))
            throw new ConflictException($"El teléfono {num} ya está registrado.");

        await EnsureDuenioExistsAsync(dto.Duenio);

        await _repository.AddAsync(dto.ToEntity());
        return await GetByIdAsync(num);
    }

    public async Task<TelefonoDto> UpdateAsync(string num, TelefonoDto dto)
    {
        if (!string.Equals(dto.Num.Trim(), num, StringComparison.Ordinal))
            throw new BusinessRuleException("El número de teléfono no se puede modificar.");

        var telefono = await _repository.GetByIdAsync(num)
            ?? throw new NotFoundException($"No existe el teléfono {num}.");

        await EnsureDuenioExistsAsync(dto.Duenio);

        dto.ApplyTo(telefono);
        await _repository.UpdateAsync(telefono);
        return await GetByIdAsync(num);
    }

    public async Task DeleteAsync(string num)
    {
        var telefono = await _repository.GetByIdAsync(num)
            ?? throw new NotFoundException($"No existe el teléfono {num}.");

        await _repository.DeleteAsync(telefono);
    }

    private async Task EnsureDuenioExistsAsync(int cc)
    {
        if (!await _personaRepository.ExistsAsync(cc))
            throw new BusinessRuleException($"No existe una persona con la cédula {cc} para asignarle el teléfono.");
    }
}
